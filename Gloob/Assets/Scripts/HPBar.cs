using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles HP UI and HP kinematic movement.
/// </summary>
public class HPBar : MonoBehaviour 
{
    private gloob gloob;

    [SerializeField]public GameObject hpBloob;
    public GameObject explosive;
    [SerializeField] float timer, radius; 
    // [SerializeField] int curHP;
    // [SerializeField] int maxHP;
    SpriteRenderer gloobSprite;
    Transform gloobPos;
    public LinkedList<GameObject> hpList;
    [SerializeField]float angleSpeed = 10f;
    bool isReposition = false, isDamaged = false;
    [SerializeField]public Dictionary<int, int> reflectHP;
    Vector2  origin;
    public properties stats;

    // Reposition states
    private const int LEFT = -1;
    private const int RIGHT = 1;
    private const float FIRST = 0;
    private const float SHIFT = 1;
    private const float DEGREE = 36;
    private float mode;
    void Start()
    {
        // loadHPStats();
        gloob = this.gameObject.GetComponentInParent<gloob>();
        gloobPos = gloob.GetComponent<Transform>();
        gloobSprite = gloob.GetComponent<SpriteRenderer>();
        hpList = new LinkedList<GameObject>();
        initiateHP(stats.maxHP);

        // used for relecting position for hpbar  
        reflectHP = new Dictionary<int, int>();
        reflectHP.Add(0, 5);
        reflectHP.Add(1, 4);
        reflectHP.Add(2, 3);
        reflectHP.Add(3, 2);
        reflectHP.Add(4, 1);
        reflectHP.Add(5, 0);
        reflectHP.Add(6, 9);
        reflectHP.Add(7, 8);
        reflectHP.Add(8, 7);
        reflectHP.Add(9, 6);
    }

    public void saveHPStats() {
    //    SaveSystem.SaveStats(this);
    }

    public void loadHPStats() { 
        //SerializeData a = SaveSystem.loadStats();
        //stats.loadData(a.curHP, a.maxHP, a.attack, a.armor, a.baseDamage, a.projectileSpeed, a.speed);
    }

    /// <summary>
    /// Called by Unity Editor Event system when gloob takes damage.
    /// Removes the first element from the list  
    /// </summary>
    /// <param name="a">Amount of damage taken</param>
    public void takeDamage(int a){
        a = Mathf.Abs(a);
    
        for(int i = 0; i < a && hpList.Any<GameObject>(); i++){
            GameObject delete = removeLastNode(hpList);
            GameObject temp = new GameObject("temp");
            delete.transform.SetParent(temp.transform);
            Destroy(temp);
        }
        if (stats.curHP != 1)
        {
            isDamaged = true;
            isReposition = true;
        }
    }
    void setInitialPos() {
        LinkedListNode<GameObject> node = hpList.First;
        node.Value.transform.localScale = new Vector2(1.25f, 1.25f);
        for (int i = 0; i < stats.curHP && node!= null; i++) {
            node.Value.transform.position = setPointOnCircle(DEGREE, i);
            node = node.Next;
        }
    }
    /// <summary>
    /// Destroies current hp bar and re-initializes HP bar. 
    /// </summary>
    /// <param name="value">current HP</param>
    void initiateHP(int value){
        radius = gloob.GetComponent<BoxCollider2D >().size.x * 0.7f;
        Vector2 position = grabPosition(gloob.gameObject);
        while (hpList.Any<GameObject>()) {
            Destroy(removeLastNode(hpList));
        }
        if(value >0){
            for(int i = 0; i < stats.curHP; i++){
                GameObject a = Instantiate(hpBloob, transform);
                a.GetComponent<Transform>().position = new Vector2(setX(radius, ((DEGREE * i))) + position.x, setY(radius, ((DEGREE * i))) + position.y);
                if (i == 0){
                    origin = a.transform.position;
                }
                LinkedListNode<GameObject> node = new LinkedListNode<GameObject>(a);
                hpList.AddLast(a);
            }
            hpList.First.Value.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        }
    }
    // may need to rewrite this? Only 1 reference may not need another function?
    Vector2 grabPosition(GameObject a){
        Transform p = a.GetComponent<Transform>();
        return new Vector2(p.position.x, p.position.y);
    }

    /// <summary>
    /// Rotates each hp bloob slightly
    /// </summary>
    /// <param name="speed">Speed of rotation</param>
    void rotatePosition(float speed){
        foreach(GameObject i in hpList){
            
            i.transform.RotateAround(gloob.transform.position, Vector3.forward, speed *Time.deltaTime);
            }
        }
    void Update()
    {
        if(pauseGame.isPausing)
            return;
        int direction = gloobSprite.flipX == true ? -1: 1;
        
        if(!isDamaged){
            if (Input.GetKeyDown(KeyCode.T)) {
                GameObject a = Instantiate(explosive);
                alterHP(2, a);
            }

            if(Input.GetButtonDown("Fire1") && timer <= 0 && stats.curHP >= 2 &&!isReposition){
                GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1.3f);
                GetComponent<AudioSource>().Play();
                alterHP(-1, null);
                isReposition = true;
                mode = FIRST;
                timer = stats.attackSpeed;
                Fire(direction);
            }
            if (!isReposition && Input.GetButtonDown("RotateRight")) {
                shiftClockwiseOnce(direction);
                mode = FIRST;
            }
            else if (!isReposition && Input.GetButtonDown("RotateLeft")) {
                shiftCounterClockwiseOnce(direction);
                mode = SHIFT;
            }
        }
        if(isReposition){
            if(mode == SHIFT)
                direction *= -1;
            
            repositionBar(direction, mode);
        }
        timer -= Time.deltaTime;
    }

    public void purchaseBloob() { 
        GameObject a = Instantiate(hpBloob);
        alterHP(1, a);
    }

    public void purchaseBomb()
    {
        GameObject a = Instantiate(explosive);
        alterHP(1, a);
    }
    void Fire(int direction) {
        GameObject temp = removeFirstNode(hpList);
        string key = temp.name.Substring(0, 2);
        if (key == "HP") {
            //Debug.Log("ammo found");
            temp.GetComponent<HPBloob>()?.Fire(direction, stats.projectileSpeed, stats.baseDamage);
            return;
        }
        else if (key == "EX") { 
            
            //Debug.Log("explosive found");
            temp.GetComponent<explosiveBloob>()?.Fire(direction, stats.projectileSpeed, stats.baseDamage);
            return;
        }
        Debug.Log("Unknown gameObject");
    }
    void shiftClockwiseOnce(int direction) { 
        GameObject temp = removeFirstNode(hpList);
        hpList.AddLast(temp);
        isReposition = true;
        int location = stats.curHP;
        if (direction == LEFT) {
            location = reflectHP[stats.curHP ];
        }
        temp.transform.position = setPointOnCircle(DEGREE, location);
        temp.transform.localScale = new Vector3(1, 1, 1);
    }
    void shiftCounterClockwiseOnce(int direction) {
        hpList.First.Value.transform.localScale = new Vector3(1, 1, 1);

        GameObject temp = removeLastNode(hpList);
        hpList.AddFirst(temp);
        isReposition = true;
        int location = 9;
        if (direction == LEFT) {
            location = reflectHP[9];
        }
        temp.transform.position = setPointOnCircle(DEGREE, location);
        
    }
    /// <summary>
    /// Invoked when hp changes.
    /// </summary>
    /// <param name="a">change in hp</param>
    public void alterHP(int a, GameObject bloob){
        stats.curHP += a; 
        if(stats.curHP >= stats.maxHP){
            a = stats.maxHP - (stats.curHP-a) ;
            stats.curHP = stats.maxHP;
        }
        if(stats.curHP <=0){
            gameOver();
        }

        if(a > 0){
            addBar(a, bloob);
        }

    }

    public void gameOver() {
        stats.reset();
        SceneManager.LoadScene("MainMenu");
    }
    /// <summary>
    /// Reflects all hp bloob position on the y axis. 
    /// Invoked when gloob changes direction.
    /// </summary>
    public void reflection(){
        foreach(GameObject i in hpList){
            float x = gloob.transform.position.x;
            float temp = i.transform.position.x;
            float d = (temp - x)*2; 
            i.transform.position = new Vector2(i.transform.position.x - d, i.transform.position.y);
        }
        
    }
    /// <summary>
    /// Adds a bloob to the hp bar. NEED TO REFACTOR 
    /// "Interrupts" rotation and re-initializes hp bar if needed.
    /// </summary>
    /// <param name="add"></param>
    void addBar(int add, GameObject bloob){

        int index;
        bloob.transform.parent = transform;
        bloob.transform.localScale = new Vector3(1f, 1f, 1f);
        bloob.layer = LayerMask.NameToLayer("hp");
        if (isReposition) {
            isReposition = false;
            isDamaged = false;
            hpList.AddLast(bloob);
            
            setInitialPos();
            if (gloobSprite.flipX)
            {
                reflection();
            }
            return;
        }

        hpList.AddLast(bloob);

        for (int k = 1; k < add; k++)
        {
            Debug.Log("More than one is added");
            index = stats.curHP - add + k;
            if (gloobSprite.flipX)
            {
                reflectHP.TryGetValue(index, out index);
            }
            GameObject newBloob = Instantiate(bloob, transform);
            newBloob.transform.position = setPointOnCircle(DEGREE, index);
            hpList.AddLast(new LinkedListNode<GameObject>(newBloob));
        }
        setInitialPos();
        if (gloobSprite.flipX)
        {
            reflection();
        }
    }

    float determinePosition(int direction, float mode) {
        if (direction == LEFT && mode == FIRST) {
            return 180;
        }
        else if(mode == FIRST)
            return 0;
        if (direction == LEFT && mode == SHIFT)
        {
            return 0;
        }
        return 180;
        }
    /// <summary>
    /// Determines how to rotate the hp bar or when to stop.
    /// </summary>
    /// <param name="i"> direction of rotation </param>
    /// <param name="displaced"> needs to be 0. HP bar completely breaks if not 0 (must remove later)</param>
    void repositionBar(int direction, float displaced){
        if (!hpList.Any<GameObject>()) {
            return;
        }
        displaced = determinePosition(direction, displaced);
        origin = setPointOnCircle(displaced, 1);
        Vector3 curPos = hpList.First.Value.transform.position;

        //may not work on low frame rate :( 
        if(Vector3.Distance(curPos, origin) <= 0.0175){
            hpList.First.Value.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            isReposition = false;
            isDamaged = false;
            return;
        }
        rotatePosition((angleSpeed* direction* -1 * 10f) );
    }
    Vector3 setPointOnCircle(float displaced, int index) { 
        return new Vector3(setX(radius, displaced * index) + gloobPos.position.x, setY(radius, (displaced * index)) + gloobPos.position.y);
    }

    /// <summary>
    /// Removes Last element from LinkedList and returns the gameobject
    /// </summary>
    /// <param name="hpList"></param>
    /// <returns>GameObject of Last element</returns>
    public GameObject removeLastNode(LinkedList<GameObject> hpList) {
        if (!hpList.Any<GameObject>()) {
            return null;
        }
        GameObject temp = hpList.Last.Value;
        hpList.RemoveLast();
        return temp;
    }
    /// <summary>
    /// Removes First element from LinkedList and returns the gameobject
    /// </summary>
    /// <param name="hpList"></param>
    /// <returns>GameObject of First element</returns>
    public GameObject removeFirstNode(LinkedList<GameObject> hpList) {
        if (!hpList.Any<GameObject>()) {
            return null;
        }
        GameObject temp = hpList.First.Value;
        hpList.RemoveFirst();
        return temp;
    }
    /// <summary>
    /// Sets the x position in the cirular hp bar.
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="degree"></param>
    /// <returns></returns>
    public float setX( float radius, float degree){
        float result =  radius * Mathf.Cos(degree*Mathf.Deg2Rad);
        return result;
        
    }

    /// <summary>
    /// Sets the u position in the cirular hp bar.
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="degree"></param>
    /// <returns></returns>
    public float setY(float radius, float degree){
        float result = radius * Mathf.Sin(degree*Mathf.Deg2Rad);
        return result ;
        
    }
    #region Properties
    public int MaxHP{
        get{
            return stats.maxHP;
        }
        set{
            stats.maxHP = value;
        }
    }
    public int CurHP{
        get{
            return stats.curHP;
        }
        set{
            stats.curHP = value;
        }
    }
    #endregion

}
