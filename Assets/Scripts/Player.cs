using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : Character {
	private static int alivePlayer = 0;

	public enum Ability {
		Move		 = 0x0001,
		Shotgun		 = 0x0002,
		Laser		 = 0x0004,
		Bomb		 = 0x0008,
		Dash		 = 0x0010,
		Magnet		 = 0x0020,
		BulletTime	 = 0x0040,
		Chain		 = 0x0080,
		GoSupport	 = 0x0100,
		GoDPS		 = 0x0200,
		TakeDamage	 = 0x0400,
		LoadLaser	 = 0x0800,
		All			 = 0x0FFF
	}

	public enum PlayerState {
		DPS,
		Support
	}

	private static Vector3 topRight = Vector3.Normalize (new Vector3 (1, 1, 0));

	public int playerNumber;
	public PlayerState state = PlayerState.DPS;
	public float switchStateCooldown = 0f;
	private float nextSwitchStateTime = 0f;

	public float respawnDelay = 10f;

	public float bulletTimeDuration = 2f;
	public float bulletTimeFactor = 0.5f;
	public float bulletTimeCooldown = 1f;
	private bool bulletTime = false;
	private float bulletTimeEndTime;

	public float dashDistance = 0.5f;
	public float dashSpeed = 16f;
	public float dashCooldown = 0.5f;
	public bool dash {
		get;
		private set;
	}
	public int projectilesAbsorbed {
		get;
		private set;
	}
	private float dashEndTime;
	private Vector3 dashVector;

	public float supportDamageReduction = 0.25f;
	public float supportSpeed = 8f;
	public float dpsSpeed = 4f;

	public float maxLaserLoad = 30f;
	private float laserLoad = 0f;
	public float laserSpeed = 2f;
	public float laserUnloadTime = 5f;

	private int abilities;

	public GameObject magnet {
		get;
		private set;
	}
	private GameObject smartBomb;
	private GameObject laser;

	public Image laserGauge;
	public Image specialGauge;
	public Image healthGauge;

	/*================Message de KV====================
	 * 
	 * Je vais désactiver les animations pour l'instant pour plus de clarté, faudras revoir comment les implémenter pour la gold
	*/

	//FXs
	public GameObject fx_Cancel;
	public Sprite DPS;
	public Sprite Support;

	protected override void AwakeCharacter () {
		if (invincible) {
			SetAbilities (Ability.TakeDamage, false);
		}
		dash = false;
		//gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Rocket"+playerNumber);
		_isEnemy = false;
		if (laser == null) {
			laser = (GameObject)Instantiate (Resources.Load<GameObject> ("Prefabs/Laser"));
			laser.GetComponent<Laser> ().Stop ();
			laser.transform.parent = transform;
			laser.transform.position = transform.position;
		}
		if (smartBomb == null) {
			smartBomb = (GameObject)Instantiate (Resources.Load<GameObject> ("Prefabs/SmartBomb"));
			smartBomb.SetActive (false);
			smartBomb.transform.parent = transform;
			smartBomb.transform.position = transform.position;
		}
		if (magnet == null) {
			magnet = (GameObject)Instantiate (Resources.Load<GameObject> ("Prefabs/Magnet"));
			magnet.SetActive (false);
			magnet.transform.parent = transform;
			magnet.transform.position = transform.position;
		}
	}

	protected override void StartCharacter () {
		alivePlayer++;
		Reset ();
	}

	public void Reset () {
		speed = state == PlayerState.DPS ? dpsSpeed : supportSpeed;
		health = maxHealth;
		UpdateGauges ();//*
		if (!((GameObject.FindObjectOfType<GameSettings> () != null) ? GameSettings.tutorial : false)) {
			laserLoad = maxLaserLoad;
			SetAbilities (Ability.All, true);
		}/*/
		laserLoad = maxLaserLoad;
		SetAbilities (Ability.All, true);
		//*/
		magnet.SetActive (false);
		laser.GetComponent<Laser> ().Stop ();
		SetCanShoot (false);
	}

	protected override void UpdateCharacter () {
		//------------------------------ Gestion des mouvements ----------------------------------------
		float dx = Input.GetAxis ("Horizontal_P" + playerNumber);
		float dy = Input.GetAxis ("Vertical_P" + playerNumber);

		Vector3 deltaPos = Vector3.zero;
		if (CheckAbility (Ability.Move) && (dx != 0 || dy != 0)) {
			if (!dash) {
				deltaPos = Vector3.Normalize (new Vector3 (Mathf.Abs (dx), Mathf.Abs (dy), 0));

				if (Vector3.Dot (Vector3.up, deltaPos) > Vector3.Dot (topRight, deltaPos)) {
					deltaPos = new Vector3 (0, Mathf.Sign (dy), 0);
					//gameObject.GetComponent<Animator> ().Play (Mathf.Sign (dy) < 0 ? "down" : "up");
				} else if (Vector3.Dot (Vector3.right, deltaPos) > Vector3.Dot (topRight, deltaPos)) {
					deltaPos = new Vector3 (Mathf.Sign (dx), 0, 0);
					//gameObject.GetComponent<Animator> ().Play (Mathf.Sign (dx) < 0 ? "left" : "right");
				} else {
					deltaPos = Vector3.Normalize (new Vector3 (Mathf.Sign (dx), Mathf.Sign (dy), 0));
					//gameObject.GetComponent<Animator> ().Play ((Mathf.Sign (dy) < 0 ? "down_" : "up_") + (Mathf.Sign (dx) < 0 ? "left" : "right"));
				}
			} else {
				deltaPos = dashVector;
			}

			gameObject.transform.Translate (deltaPos * IngameTime.deltaTime * speed);
		} else {
			//.GetComponent<Animator> ().Play ("idle");
		}

		//------------------------------ Gestion des capacites ----------------------------------------
		if (bulletTime) {
			if (IngameTime.globalTime >= bulletTimeEndTime) {
				bulletTime = false;
				IngameTime.MultiplyFactor (1f / bulletTimeFactor);
			}
		}
		if (dash) {
			speed = dashSpeed;
			if (IngameTime.time >= dashEndTime) {
				dash = false;
				transform.GetChild(1).localScale = Vector3.one;
				speed = state == PlayerState.DPS ? dpsSpeed : supportSpeed;
			}
		}
		if (laser.GetComponent<Laser> ().isActiveAndEnabled) {
			speed = laserSpeed;
			if ((laserLoad -= maxLaserLoad * IngameTime.deltaTime / laserUnloadTime) <= 0f) {
				laserLoad = 0f;
				AkSoundEngine.PostEvent ("laser_empty", gameObject);
				laser.GetComponent<Laser> ().Stop ();
				speed = state == PlayerState.DPS ? dpsSpeed : supportSpeed;
			}
		}

		//------------------------------ Gestion des inputs de capacites ----------------------------------------
		switch (state) {
		case PlayerState.DPS:
			if (Input.GetButtonDown ("Fire1_P" + playerNumber) && CheckAbility (Ability.Laser)) {
				if (laserLoad > 0f) {
					laser.SetActive (true);
					laser.GetComponent<Laser> ().Shoot ();
				}
			}
			if (Input.GetButtonUp ("Fire1_P" + playerNumber)) {
				if (laser != null) {
					laser.GetComponent<Laser> ().Stop ();
				}
				speed = dpsSpeed;
				//laser.SetActive (false);
			}
			if (Input.GetButton ("Fire2_P" + playerNumber) && CheckAbility (Ability.Shotgun)) {
				SetCanShoot (true);
			}
			if (Input.GetButtonUp ("Fire2_P" + playerNumber)) {
				SetCanShoot (false);
			}
			if (Input.GetButtonDown ("Fire3_P" + playerNumber) && CheckAbility (Ability.Bomb)) {
				smartBomb.SetActive (true);
			}
			break;
		case PlayerState.Support:
			if (Input.GetButtonDown ("Fire1_P" + playerNumber) && CheckAbility (Ability.Magnet)) {
				magnet.SetActive (true);
			}
			if (Input.GetButtonUp ("Fire1_P" + playerNumber)) {
				magnet.SetActive (false);
			}
			if (Input.GetButtonDown ("Fire2_P" + playerNumber) && CheckAbility (Ability.Dash)) {
				if (!dash) {
					if (IngameTime.time > dashEndTime + dashCooldown) {
						dash = true;
						//transform.GetChild (1).GetComponent<CircleCollider2D> ().radius = 2;
						transform.GetChild (1).localScale = Vector3.one * 10f;
						dashEndTime = IngameTime.time + dashDistance / dashSpeed;
						speed = dashSpeed;
						dashVector = deltaPos;
						AkSoundEngine.PostEvent ("dash", gameObject);
					}
				}
			}
			if (Input.GetButtonDown ("Fire3_P" + playerNumber) && CheckAbility (Ability.BulletTime)) {
				if (!bulletTime) {
					if (Time.time > bulletTimeEndTime + bulletTimeCooldown) {
						bulletTime = true;
						bulletTimeEndTime = IngameTime.globalTime + bulletTimeDuration;
						IngameTime.MultiplyFactor (bulletTimeFactor);
					}
				}
			}
			break;
		}
		if (Input.GetButtonDown ("DPS_P" + playerNumber) && CheckAbility (Ability.GoDPS) && state != PlayerState.DPS) {
			if (IngameTime.globalTime > nextSwitchStateTime) {
				SetState (PlayerState.DPS);
			}
		} else if (Input.GetButtonDown ("Support_P" + playerNumber) && CheckAbility (Ability.GoSupport) && state != PlayerState.Support) {
			if (IngameTime.globalTime > nextSwitchStateTime) {
				SetState (PlayerState.Support);
			}
		}

		UpdateGauges ();
	}

	public void SetState (PlayerState nstate) {
		if (nstate == PlayerState.DPS) {
			speed = dpsSpeed;
			state = PlayerState.DPS;
			magnet.SetActive (false);
			//gameObject.GetComponent<SpriteRenderer> ().color = Color.magenta;
			gameObject.GetComponent<SpriteRenderer>().sprite = DPS;
			nextSwitchStateTime = IngameTime.globalTime + switchStateCooldown;
			AkSoundEngine.PostEvent ("switch_to_dps", gameObject);
		} else {
			SetCanShoot (false);
			if (!dash)
				speed = supportSpeed;
			laser.GetComponent<Laser> ().Stop ();
			state = PlayerState.Support;
			//gameObject.GetComponent<SpriteRenderer> ().color = Color.cyan;
			gameObject.GetComponent<SpriteRenderer>().sprite = Support;
			nextSwitchStateTime = IngameTime.globalTime + switchStateCooldown;
			AkSoundEngine.PostEvent ("switch_to_support", gameObject);
		}
	}

	public void SetLaserMaxLoad (float value) {
		if (value <= 0f)
			return;
		maxLaserLoad = value;
		if (laserLoad > value)
			laserLoad = value;
	}

	public float LaserLoad () {
		return Mathf.Clamp01 (laserLoad / maxLaserLoad);
	}

	public void EatBullet (Vector3 pos) {
		if (dash) {
			if (state == PlayerState.Support) {
				if (CheckAbility (Ability.LoadLaser)) {
					if (laserLoad < maxLaserLoad && ++laserLoad > maxLaserLoad)
						AkSoundEngine.PostEvent ("laser_loaded", gameObject);
					if (laserLoad > maxLaserLoad)
						laserLoad = maxLaserLoad;
				}
			}
			Instantiate (Resources.Load<GameObject>("Particules/Cancel"),pos-Vector3.forward*0.1f,Quaternion.identity,ViewportHandler.viewport.transform);
			projectilesAbsorbed++;
		}
	}

	protected override void Death () {
		AkSoundEngine.PostEvent ("death", gameObject);
		if (--alivePlayer <= 0) {
			AkSoundEngine.PostEvent ("game_over", GameSettings.game_settings);
			GameSettings.SetState (GameSettings.GameState.GameOver);
		} else {
			StartCoroutine ("RespawnPlayer", this);
		}
		Reset ();
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		this.enabled = false;
	}

	IEnumerator RespawnPlayer (Player player) {
		float respawnTime = IngameTime.globalTime + player.respawnDelay;
		while (IngameTime.globalTime < respawnTime) {
			if (alivePlayer <= 0) {
				//Debug.Log ("alivePlayer <= 0");
				respawnTime = 0f;
			} else {
				//Debug.Log ("time till respawn : " + (respawnTime - IngameTime.time));
				yield return null;
			}
		}
		if (alivePlayer <= 0) {
			Destroy (player.gameObject);
		} else {
			Bounds b = ViewportHandler.viewport.GetComponent<BoxCollider2D> ().bounds;
			player.gameObject.transform.position = ViewportHandler.viewport.transform.position + new Vector3 ((player.playerNumber % 2 == 0 ? 1f : -1f) * (b.extents.x - 2f * GetComponent<SpriteRenderer> ().bounds.extents.x), -b.extents.y);
			player.enabled = true;
			player.Reset ();
			gameObject.GetComponent<SpriteRenderer> ().enabled = true;
			player.SetInvincible ();
			alivePlayer++;
			//Debug.Log ("respawned!");
		}
	}

	public override bool TakeDamage (float value, bool activeInvincibility = true) {
		if (dash || !CheckAbility (Ability.TakeDamage))
			return false;
		AkSoundEngine.PostEvent ("character_hit", gameObject);
		bool res = base.TakeDamage (value * (state == PlayerState.Support ? supportDamageReduction : 1f), activeInvincibility);
		return res;
	}

	void UpdateGauges () {
		//Debug.Log (gameObject + " health : " + health);
		if(laserGauge!=null)
			laserGauge.fillAmount = LaserLoad ();
		if(healthGauge!=null)
			healthGauge.fillAmount = health / maxHealth;
		if (specialGauge != null) {
			specialGauge.fillAmount = state == PlayerState.DPS ? (smartBomb.GetComponent<SmartBomb> ().GetLoad ()) : Mathf.Clamp01 ((bulletTimeEndTime - IngameTime.time) / bulletTimeCooldown);
		//	specialGauge.transform.localScale = new Vector3 (state == PlayerState.DPS ? (smartBomb.GetComponent<SmartBomb> ().GetLoad ()) : Mathf.Clamp01 ((bulletTimeEndTime - IngameTime.time) / bulletTimeCooldown), 1f, 1f);
		}
	}

	public void SetAbilities (Ability ability, bool value) {
		if (value) {
			abilities |= (int)ability;
		} else {
			abilities &= ~((int)ability);
		}
	}

	public bool CheckAbility (Ability ability) {
		return (abilities & (int)ability) != 0;
	}

	public override string ToString() {
		return "Player_" + playerNumber;
	}

	void OnGUI () {
		GUIStyle style = new GUIStyle ();
		style.normal.textColor = Color.white;
		style.fontSize = 30;
		GUI.Label (new Rect (10, 10 + (playerNumber-1)*30, 200, 30), laserLoad + "/" + maxLaserLoad, style);
	}
}
