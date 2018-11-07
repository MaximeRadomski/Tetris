using UnityEngine;
using System.Collections;

public class GridBehavior : MonoBehaviour
{
    //The Grid itself
    public GameObject mainParticlesLineDestroyed;
    public static GameObject particlesLineDestroyed = null;
    public GameObject mainParticlesCubeDestroyed;
    public static GameObject particlesCubeDestroyed = null;
    public static int w = 10;
    public static int h = 25;
    public static Transform[,] grid = new Transform[w,h];
    public static int fullRows = 0;
    public static AudioSource[] audios;
	public static int NbGarbage = -1;

	public GameObject LineClearDouble, LineClearTriple, LineClearTetris, LineClearB2bTetris, LineClearCombo, LineClearTSpin, LineClearScore;
	public static GameObject _lineClearDouble, _lineClearTriple, _lineClearTetris, _lineClearB2bTetris, _lineClearCombo, _lineClearTSpin, _lineClearScore;

	static private GameObject gameManager;
	static private GameObject canvas;

	private static float _combo = 0.0f;
	private static bool _lastWasTetris = false;
	private static bool _currentIsTSpin = false;
	private static int _consecutiveB2B = 0;

    void Start()
    {
        particlesLineDestroyed = mainParticlesLineDestroyed;
        particlesCubeDestroyed = mainParticlesCubeDestroyed;
		_lineClearDouble = LineClearDouble;
		_lineClearTriple = LineClearTriple;
		_lineClearTetris = LineClearTetris;
		_lineClearB2bTetris = LineClearB2bTetris;
		_lineClearCombo = LineClearCombo;
		_lineClearTSpin = LineClearTSpin;
		_lineClearScore = LineClearScore;
        audios = GetComponents<AudioSource>();
        audios[0].volume = audios[0].volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        audios[1].volume = audios[1].volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
		gameManager = GameObject.Find("$GameManager");
		canvas = GameObject.Find("Canvas");
    }

    public static Vector2 RoundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static bool InsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < w && (int)pos.y >= 0);
    }

    public static void DeleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (particlesCubeDestroyed != null)
            {
                Instantiate(particlesCubeDestroyed, new Vector3(x, y, 0.0f), particlesCubeDestroyed.transform.rotation);
            }
			if (grid [x, y].gameObject.tag == "GarbageGroup")
				--NbGarbage;
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public static void DecreaseRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] != null)
            {
                //Move one toward bottom
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                //Update Block position
                grid[x, y-1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < h; ++i)
        {
            DecreaseRow(i);
        }
    }

    public static bool IsRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    public static void DeleteFullRows()
    {
        for (int y = 0; y < h; ++y)
        {
            if (IsRowFull(y))
            {
                if (particlesLineDestroyed != null)
                {
                    Instantiate(particlesLineDestroyed, new Vector3(4.5f, y, 0.0f), particlesLineDestroyed.transform.rotation);
                }
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                --y;
            }
        }
		if (NbGarbage == 0 && PlayerPrefs.GetInt("GameType") == 3)
			gameManager.GetComponent<GameManagerBehaviour> ().EndCleaning();
    }

    public static int CheckIfFullRows()
    {
        GameObject tmp;
		int tmpScore = 0;
        int level;
        fullRows = 0;
        for (int y = 0; y < h; ++y)
        {
            if (IsRowFull(y))
            {
                if (particlesLineDestroyed != null)
                {
                    Instantiate(particlesLineDestroyed, new Vector3(4.5f, y - 0.3f, 0.0f), particlesLineDestroyed.transform.rotation);
                }
                ++fullRows;
            }
        }
        tmp = GameObject.Find("Score");
		if (fullRows > 0)
		{
			tmp.GetComponent<ScoreBehaviour> ().AddToLines (fullRows);
			_combo += 1.0f;
		}
		else
			_combo = 0.0f;
        level = tmp.GetComponent<ScoreBehaviour>().level;
		switch (fullRows) {
			case 1:
				if (_currentIsTSpin) // Move : T-Spin Single
				{
					tmp.GetComponent<ScoreBehaviour> ().AddToScore (tmpScore = 800 * (level + 1));
					tmp.GetComponent<ScoreBehaviour> ().RemoveFromNeeded (8);
					InstantiateLineClearText (_lineClearTSpin, "T-Single");
				}
				else // Move : Single
				{
					tmp.GetComponent<ScoreBehaviour> ().AddToScore (tmpScore = 100 * (level + 1));
					tmp.GetComponent<ScoreBehaviour> ().RemoveFromNeeded (1);
				}
				audios [0].Play ();
				gameManager.GetComponent<GameManagerBehaviour> ().OneLine ();
				_lastWasTetris = false;
				break;
			case 2:
				if (_currentIsTSpin) // Move : T-Spin Double
				{
					tmp.GetComponent<ScoreBehaviour>().AddToScore(tmpScore = 1200 * (level + 1));
					tmp.GetComponent<ScoreBehaviour> ().RemoveFromNeeded (12);
					InstantiateLineClearText (_lineClearTSpin, "T-Double");
				}
				else // Move : Double
				{	
					tmp.GetComponent<ScoreBehaviour> ().AddToScore (tmpScore = 300 * (level + 1));
					tmp.GetComponent<ScoreBehaviour> ().RemoveFromNeeded (3);
					InstantiateLineClearText (_lineClearDouble);
				}
				audios [0].Play ();
				gameManager.GetComponent<GameManagerBehaviour>().TwoLines();
				_lastWasTetris = false;
                break;
			case 3: // Move : Triple
				tmp.GetComponent<ScoreBehaviour>().AddToScore(tmpScore = 500 * (level + 1));
                tmp.GetComponent<ScoreBehaviour>().RemoveFromNeeded(5);
				InstantiateLineClearText (_lineClearTriple);
				audios[0].Play();
				gameManager.GetComponent<GameManagerBehaviour>().ThreeLines();
				_lastWasTetris = false;
                break;
			case 4:
				if (_lastWasTetris == true) // Move : B2B Tetris
				{
					tmp.GetComponent<ScoreBehaviour> ().AddToScore (tmpScore = 1200 * (level + 1));
					tmp.GetComponent<ScoreBehaviour> ().RemoveFromNeeded (12);
					InstantiateLineClearText (_lineClearB2bTetris);
					++_consecutiveB2B;
					if (_consecutiveB2B == 3) // 3 B2B = 4 Tetrises in a row
						ConsecutiveTetrisesSuccess ();
				}
				else // Move : Tetris
				{
					tmp.GetComponent<ScoreBehaviour> ().AddToScore (tmpScore = 800 * (level + 1));
					tmp.GetComponent<ScoreBehaviour> ().RemoveFromNeeded (8);
					InstantiateLineClearText (_lineClearTetris);
					_consecutiveB2B = 0;
				}
				audios [0].Play ();
				//audios[1].Play(); Tetris is now said through "LineClearTetris"
				gameManager.GetComponent<GameManagerBehaviour> ().TetrisDance ();
				if (gameManager.GetComponent<GameManagerBehaviour> ().sexyEnable == true)
				{
					GameObject.Find ("BackgroundKremlin").GetComponent<BackgroundKremlinBehaviour> ().Decrease ();;
				}
				_lastWasTetris = true;
                break;
			default:
				if (_currentIsTSpin) // Move : T-Spin
				{
					tmp.GetComponent<ScoreBehaviour> ().AddToScore (tmpScore = 400 * (level + 1));
					tmp.GetComponent<ScoreBehaviour> ().RemoveFromNeeded (4);
					InstantiateLineClearText (_lineClearTSpin);
				}
				break;
        }
		if (_combo > 1.0f)
		{
			InstantiateLineClearText (_lineClearCombo, "Combo x"+(_combo-1), true);
			tmp.GetComponent<ScoreBehaviour>().AddToScore(tmpScore += 50 * (int)_combo * (level + 1)); // Move : Combo
			ComboSuccess();
		}
		if (tmpScore > 0)
			InstantiateLineClearText (_lineClearScore, tmpScore.ToString());
		_currentIsTSpin = false;
        return (fullRows);
    }

	private static void ComboSuccess()
	{
		if (_combo == 9)//9 par ce qu'on veut voir combo 8 mais il ne commence qu'à compter à partir de 2 et non 1.
		{
			if (PlayerPrefs.GetString ("UnlockedTetrominoesStyles", "10000000000000000000") [5] == '1')
			{
				gameManager.GetComponent<GameManagerBehaviour> ().DisplayPopup ("Soviet Nostalgia");
			}
			else
			{
				string tmpUnlockedTetrominoesStyles = "";
				tmpUnlockedTetrominoesStyles += PlayerPrefs.GetString ("UnlockedTetrominoesStyles", "10000000000000000000");
				tmpUnlockedTetrominoesStyles = tmpUnlockedTetrominoesStyles.Remove(5,1);
				tmpUnlockedTetrominoesStyles = tmpUnlockedTetrominoesStyles.Insert(5,"1");
				PlayerPrefs.SetString ("UnlockedTetrominoesStyles", tmpUnlockedTetrominoesStyles);
				gameManager.GetComponent<GameManagerBehaviour>().DisplayPopup("Unlocked:\nSoviet Nostalgia");
			}
		}
		if (_combo >= 3 && _consecutiveB2B == 2)
		{
			if (PlayerPrefs.GetString ("UnlockedCurtainsStyles", "10000000000000000000") [3] == '1')
			{
				gameManager.GetComponent<GameManagerBehaviour>().DisplayPopup("Fifth Reich");
			}
			else
			{
				string tmpUnlockedCurtainsStyles = "";
				tmpUnlockedCurtainsStyles += PlayerPrefs.GetString ("UnlockedCurtainsStyles", "10000000000000000000");
				tmpUnlockedCurtainsStyles = tmpUnlockedCurtainsStyles.Remove(3,1);
				tmpUnlockedCurtainsStyles = tmpUnlockedCurtainsStyles.Insert(3,"1");
				PlayerPrefs.SetString ("UnlockedCurtainsStyles", tmpUnlockedCurtainsStyles);
				gameManager.GetComponent<GameManagerBehaviour>().DisplayPopup("Unlocked:\nFifth Reich");	
			}
		}
	}

	private static void ConsecutiveTetrisesSuccess()
	{
		if (PlayerPrefs.GetString ("UnlockedCurtainsStyles", "10000000000000000000") [1] == '1')
		{
			gameManager.GetComponent<GameManagerBehaviour>().DisplayPopup("Grandma's Carpet");
		}
		else
		{
			string tmpUnlockedCurtainsStyles = "";
			tmpUnlockedCurtainsStyles += PlayerPrefs.GetString ("UnlockedCurtainsStyles", "10000000000000000000");
			tmpUnlockedCurtainsStyles = tmpUnlockedCurtainsStyles.Remove(1,1);
			tmpUnlockedCurtainsStyles = tmpUnlockedCurtainsStyles.Insert(1,"1");
			PlayerPrefs.SetString ("UnlockedCurtainsStyles", tmpUnlockedCurtainsStyles);
			gameManager.GetComponent<GameManagerBehaviour>().DisplayPopup("Unlocked:\nGrandma's Carpet");	
		}
	}

	private static void InstantiateLineClearText(GameObject lineClearText, string customText = null, bool isCombo = false)
	{
		var tmp = Instantiate (lineClearText, new Vector3(0.0f, 4.0f, 0.0f), _lineClearDouble.transform.rotation);
		tmp.transform.SetParent(canvas.transform, false);
		if (customText != null)
		{
			tmp.GetComponent<UnityEngine.UI.Text> ().text = customText;
			if (isCombo == true)
				tmp.GetComponent<AudioSource>().pitch = 1.0f + (_combo - 2.0f) / 3.0f; // (_combo - 2) car le son de combo commence à partir d'un combo de 2
		}
	}

	public static void TSpinOccured()
	{
		_currentIsTSpin = true;
	}

	public static IEnumerator SetCleaningArea(GameObject garbage)
	{
		int i;
		NbGarbage = 0;
		for (int y = 14; y >= 0; --y) //y = 14
		{
			for (int x = 0; x < w; ++x)
			{
				i = Random.Range(0, 2);
				if (i == 1)
				{
					var tmp = Instantiate (garbage, new Vector3 (x, y, 0), garbage.transform.rotation);
					grid [x, y] = tmp.transform;
					++NbGarbage;
					yield return new WaitForSeconds(0.01f);
				}
			}
		}
	}
}

/*
 	Action							Point Value
 	
 -  Single/Mini T-spin				100×level
	Mini T-Spin Single				200×level
 -  Double							300×level
 -  T-Spin							400×level
 -  Triple							500×level
 -  Tetris/T-Spin Single			800×level
	B2B T-Spin Mini					150×level
 -  B2B T-Spin Single/B2B Tetris/T-Spin Double	1,200×level
	T-Spin Triple					1,600×level
	B2B T-Spin Double				1,800×level
	B2B T-Spin Triple				2,400×level
 -  Combo							50×combo count×level (singles only for 20)
 -  Soft drop						1 point per cell (Max of 20)
 -  Hard drop						2 points per cell (Max of 40)


	Action							Awarded Line Clears

 -  Single							1
 -  Double							3
 -  T-Spin							4
 -  Triple							5
	T-Spin Mini						1
	T-Spin Mini Single				2
 -  Tetris / T-Spin Single			8
 -  T-Spin Double					12
	T-Spin Triple					16
 -  Back-to-Back Bonus				0.5 x Total Line Clears

*/