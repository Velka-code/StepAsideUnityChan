using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    //carPrefabを入れる
    public GameObject carPrefab;
    //coinPrefabを入れる
    public GameObject coinPrefab;
    //cornPrefabを入れる
    public GameObject conePrefab;

    public GameObject runPointPrefab;

    //スタート地点
    //private int startPos = 80;
    private int startPos = 50;
    //ゴール地点
    private int goalPos = 360;
    //アイテムを出すx方向の範囲
    private float posRange = 3.4f;

    //carオブジェクトを配列に入れる(課題のため追加）
    private GameObject[] cars;
    //coinsオブジェクトを配列に入れる(課題のため追加）
    private GameObject[] coins;
    //conesオブジェクトを配列に入れる(課題のため追加）
    private GameObject[] cones;

    //Unityちゃんのオブジェクト(課題のため追加）カメラ位置からunity-chanの位置を取得するため
    private GameObject unitychan;

    //ダミーのrunPointを生成し、UnityChanController.csにて当たり判定によりカウントを取得するため
    public UnityChanController unitychancontroller;
    //当たり判定のカウント値が更新されたときの値を取得するため比較用にgetRunPointNowを用意
    private int getRunPointNow = 0;

    private int deadline = 5;

    //課題：不要になったアイテムを順次破棄する
    //unityちゃんが通り過ぎて画面外に出たアイテムを直ちに破棄するためunityちゃんの位置を取得して
    //生成されたオブジェクトは配列にいれ、通り過ぎたものから順次破棄していく。

    void Start()
    {
        //別のスクリプトから変数を読み出す
        this.unitychan = GameObject.Find("unitychan");
        unitychancontroller = unitychan.GetComponent<UnityChanController>();

        //スタート時に50間隔にダミーのrunPointを配置。このPointを通過するたびに前方に障害物を追加する
        for (int i = 0; i < goalPos; i += 50 )
        {
            GameObject runPoint = Instantiate(runPointPrefab);
            runPoint.transform.position = new Vector3(0, runPoint.transform.position.y, i);
        }
           
    }

    

    // 動的に障害物を追加する（課題のため追加）
    void Update()
    {
        //ダミーrunPointを通過したカウント数を取得
        int getRunPoint = unitychancontroller.SetRunPoint();
        //unity-chanの現在位置を取得
        int getPos = (int)unitychan.transform.position.z;

        //取得したカウント値が更新されたかを判断。そうしないと大量に生成される。
        if(getRunPoint != getRunPointNow)
        {
            ItemGenerate(getRunPoint);
            getRunPointNow = getRunPoint;
        }


        //車のオブジェクトをunity-chanの位置からdeadlineを通り越したら削除する。（課題のため追加）
        if (this.cars != null)
        {
            for (int i = 0; i < this.cars.Length; i++)
            {
                if (this.cars[i] != null)
                {
                    if (this.cars[i].transform.position.z < getPos - deadline)
                    {
                        //通過した障害物を削除
                        Destroy(this.cars[i]);
                    }
                    else if(this.cars[i].transform.position.z > goalPos)
                    {
                        //ゴールより先に生成された障害物を削除
                        Destroy(this.cars[i]);
                    }
                }

            }
        }

        //コーンのオブジェクトをunity-chanの位置からdeadlineを通り越したら削除する。（課題のため追加）
        if (this.cones != null)
        {
            for (int i = 0; i < this.cones.Length; i++)
            {
                if (this.cones[i] != null)
                {
                    if (this.cones[i].transform.position.z < getPos - deadline)
                    {
                        Destroy(this.cones[i]);
                    }
                    else if(this.cones[i].transform.position.z > goalPos)
                    {
                        Destroy(this.cones[i]);
                    }
                }

            }
        }

        //コインのオブジェクトをunity-chanの位置からdeadlineを通り越したら削除する。（課題のため追加）
        if (this.coins != null)
        {
            for (int i = 0; i < this.coins.Length; i++)
            {
                if (this.coins[i] != null)
                {
                    if (this.coins[i].transform.position.z < getPos - deadline)
                    {
                        Destroy(this.coins[i]);
                    }
                    else if (this.coins[i].transform.position.z > goalPos)
                    {
                        Destroy(this.coins[i]);
                    }
                }
            }

        }
    }


    //障害物生成用の関数
    void ItemGenerate(int runPointCount)
    {
        //一定の距離ごとにアイテムを生成
        for (int i = startPos * runPointCount; i < startPos * runPointCount + 50; i += 10)
        {
            //どのアイテムを出すのかをランダムに設定
            int num = Random.Range(1, 11);
            if (num <= 2)
            {
                //コーンをx軸方向に一直線に生成
                for (float j = -1; j <= 1; j += 0.4f)
                {
                    GameObject cone = Instantiate(conePrefab);
                    cone.transform.position = new Vector3(4 * j, cone.transform.position.y, i);

                    //生成された車のオブジェクトを配列に入れる(課題のため追加）
                    cones = GameObject.FindGameObjectsWithTag("TrafficConeTag");
                }
            }
            else
            {
                //レーンごとにアイテムを生成
                for (int j = -1; j <= 1; j++)
                {
                    //アイテムの種類を決める
                    int item = Random.Range(1, 11);
                    //アイテムを置くZ座標のオフセットをランダムに設定
                    int offsetZ = Random.Range(-5, 6);
                    //60%コイン配置:30%車配置:10%何もなし
                    if (1 <= item && item <= 6)
                    {
                        //コインを生成
                        GameObject coin = Instantiate(coinPrefab);
                        coin.transform.position = new Vector3(posRange * j, coin.transform.position.y, i + offsetZ);

                        //生成された車のオブジェクトを配列に入れる(課題のため追加）
                        coins = GameObject.FindGameObjectsWithTag("CoinTag");
                    }
                    else if (7 <= item && item <= 9)
                    {
                        //車を生成
                        GameObject car = Instantiate(carPrefab);
                        car.transform.position = new Vector3(posRange * j, car.transform.position.y, i + offsetZ);

                        //Debug.Log("car:" + car.transform.position.z);
                        //生成された車のオブジェクトを配列に入れる(課題のため追加）
                        cars = GameObject.FindGameObjectsWithTag("CarTag");

                    }
                }
            }
        }

    }

}


