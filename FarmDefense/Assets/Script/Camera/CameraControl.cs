using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    /* 定数 */
    private const float kDistance = 4.0f;   // ターゲットとカメラとの距離
    private const float kShiftPosY = 1.2f;   // ターゲット中心から上にずらす量
    private const float kAxisMinThershold = 0.2f; // 入力情報の最小のしきい値:無視する割合
    private const float kAxisMaxThershold = 0.8f; // 入力情報の最大のしきい値:1.0とみなす割合
    private const float kSpeed = 0.75f;
    // FIXME: 名前いい感じに変更
    private float kRangeCursorDot = Mathf.Cos(10 * Mathf.Deg2Rad);   // カーソル内とする内積の範囲
    private const float kCursorLimitDistance = 30.0f;
    private const float kCursorLimitSqrDistance = kCursorLimitDistance * kCursorLimitDistance;

    /* 変数 */
    private GameObject _target;         // ターゲットのオブジェクト情報
    private Vector3 _centerPos;         // 中心座標
    private Vector3 _cameraPos;         // 中心座標
    private bool _isUpdownSwing;        // 上下にカメラを揺らしているか
    private bool _isLeftrightSwing;     // 左右に入力したか
    private bool _isUpdownInput;        // 上下に入力したか
    private bool _isReset;              // リセットしたか

    float _angleVert = 0.0f;
    Quaternion _rotSide = Quaternion.AngleAxis(0, Vector3.up);
    Vector3 kBaseVec = new Vector3(0, 0, 1);

    // FIXME: なんかいい感じの変数名に変更
    [SerializeField] private GameObject _canvas;    // キャンバス
    [SerializeField] private GameObject _barPrefab; // 元オブジェクト
    private List<GameObject> _cursorObjs;    // カーソルとあうオブジェクトの情報たち
    private GameObject _hpBarObj;            // HPバーの対象となるオブジェクト
    private GameObject _hpBar;               // HPバー自体のオブジェクト

    private void Start()
    {
        // ターゲット(プレイヤー)から情報取得
        _target = GameObject.Find("Player");

        /* 初期設定 */
        // 中心座標
        _centerPos = _target.transform.position;
        // カメラ座標
        _cameraPos = _centerPos + new Vector3(0, kShiftPosY, -kDistance);
        // 回転していないに
        _isUpdownSwing = false;
        _isLeftrightSwing = false;
        // 入力していないに
        _isUpdownInput = false;
        // リセットしていないに
        _isReset = false;

        _cursorObjs = new List<GameObject>();
        // カーソルに当たる情報に農場を追加
        for (int i = 0; i < FarmManager.kFarmNum; ++i)
        {
            _cursorObjs.Add(GameObject.Find("Farm" + i));
        }
    }

    private void Update()
    {
        RotLeftright();
        RotUpdown();

        ResetDirection();
    }

    private void FixedUpdate()
    {
        ReturnRotUpdown();

        Move();

        Cursor();
    }

    /// <summary>
    /// カメラの正面方向を取得
    /// </summary>
    /// <returns>正面方向</returns>
    public Vector3 GetFront()
    {
        return transform.forward;
    }

    /// <summary>
    /// カメラの右方向を取得
    /// </summary>
    /// <returns>右方向</returns>
    public Vector3 GetRight()
    {
        return transform.right;
    }

    public void AddHpBarInfo(GameObject obj)
    {
        _cursorObjs.Add(obj);
    }

    public void SubHpBarInfo(GameObject obj)
    {
        _cursorObjs.Remove(obj);
    }

    /// <summary>
    /// 左右の回転
    /// </summary>
    private void RotLeftright()
    {
        // 入力値の取得 
        float inputRate = Input.GetAxis("HorizontalRight");

        // 入力されていないなら終了
        if (-kAxisMinThershold < inputRate && inputRate < kAxisMinThershold)
        {
            _isLeftrightSwing = false;
            return;
        }

        inputRate = LimitValue(inputRate, kAxisMinThershold, kAxisMaxThershold);

        // 入力していることに
        _isLeftrightSwing = true;

        _rotSide = _rotSide * Quaternion.AngleAxis(inputRate * kSpeed, Vector3.up);
    }

    /// <summary>
    /// 上下の回転
    /// </summary>
    private void RotUpdown()
    {
        // 入力値の取得
        float inputRate = Input.GetAxis("VerticalRight");

        // 入力されていないなら終了
        if (-kAxisMinThershold < inputRate && inputRate < kAxisMinThershold)
        {
            _isUpdownInput = false;
            return;
        }

        inputRate = LimitValue(inputRate, kAxisMinThershold, kAxisMaxThershold);

        // 動かしていることに
        _isUpdownSwing = true;
        // 入力したことに
        _isUpdownInput = true;

        _angleVert += inputRate * kSpeed;
        _angleVert = Mathf.Max(Mathf.Min(_angleVert, 30), -30);
    }

    /// <summary>
    /// 向いている方向をプレイヤーの向いている方向に戻す
    /// </summary>
    private void ResetDirection()
    {
        // MEMO:ボタン間違えている可能性あり
        // Yボタンを押すと方向リセット
        if (Input.GetButtonDown("StickPushRight"))
        {
            // TODO:現状Leftrightの回転を0にしているだけなのでプレイヤーの向いている方向を向くように

//            _rotLeftrightSwing = dirRad;
            _rotSide = _target.transform.rotation;
            _angleVert = 0.0f;
            _isLeftrightSwing = false;
            _isUpdownSwing = false;

            _isReset = true;
        }
    }

    /// <summary>
    /// 上下方向の回転量を元に戻す
    /// </summary>
    private void ReturnRotUpdown()
    {
        // 上下方向の入力をしていれば戻さない
        if (_isUpdownInput) return;

        // 0に近づける
        _angleVert = Mathf.Lerp(_angleVert, 0.0f, 0.05f);
        // 限りなく0に近づいたら戻したことにする
        if (-0.001f < _angleVert && _angleVert < 0.001f)
        {
            _angleVert = 0.0f;
            _isUpdownSwing = false;
        }
    }

    /// <summary>
    /// 位置の更新
    /// </summary>
    private void Move()
    {
        Transform targetTrs = _target.transform;

        // 中心を更新するか
        bool isUpdateCenter = (_centerPos != targetTrs.position);

        // 中心位置の更新
        if (isUpdateCenter)
        {
            _centerPos = Vector3.Lerp(_centerPos, targetTrs.position, 0.5f);

            // 限りなく0に近づいたらもう重なっていることに
            if ((_centerPos - targetTrs.position).sqrMagnitude < 0.001f)
            {
                _centerPos = targetTrs.position;
            }
        }

        // 回転またはリセットしていれば
        if (_isReset || _isLeftrightSwing || _isUpdownSwing || isUpdateCenter)
        {
            Quaternion rotVert = Quaternion.AngleAxis(_angleVert, transform.right);
            Vector3 dir = _rotSide * rotVert * kBaseVec;

            _cameraPos = dir * -kDistance + _centerPos;
            _cameraPos.y += kShiftPosY;
        }

        

        // 位置の代入 
        transform.position = _cameraPos;

        // オブジェクトの向き変更
        Vector3 lookPos = _centerPos;
        lookPos.y += kShiftPosY;
        transform.LookAt(lookPos);
    }

    // FIXME: いい感じの変数名に
    /// <summary>
    /// 一番近くにある正面にあるオブジェクトのHP情報を表示する
    /// </summary>
    private void Cursor()
    {
        GameObject drawingObj = null;
        float nowSqrDist = 0.0f;

        foreach (var item in _cursorObjs)
        {
            // そのオブジェクトが破壊or死亡していれば次へ
            if (!IsExist(item)) continue;

            // 自身からオブジェクトまでのベクトルを生成
            Vector3 cameraToitemVec = item.transform.position - transform.position;
            // XZ方向のみ取り出し
            Vector2 cToIVec;
            cToIVec.x = cameraToitemVec.x;
            cToIVec.y = cameraToitemVec.z;
            Vector2 front;
            front.x = transform.forward.x;
            front.y = transform.forward.z;

            // もし距離が離れすぎていたら次へ
            float sqrDist = cToIVec.sqrMagnitude;
            if (sqrDist > kCursorLimitSqrDistance) continue;

            // 正規化
            cToIVec.Normalize();
            // 内積
            float dot = Vector2.Dot(cToIVec, front);

            // 設定範囲内かの判定
            if (dot > kRangeCursorDot)
            {
                // 1回目のヒット
                if (!drawingObj)
                {
                    // そのまま代入
                    drawingObj = item;
                    nowSqrDist = sqrDist;
                }
                // 2回目以降のヒット
                else
                {
                    // 現在の距離より小さいなら代入
                    if (sqrDist < nowSqrDist)
                    {
                        drawingObj = item;
                        nowSqrDist = sqrDist;
                    }
                }
            }
        }

        // 誰にもヒットしなかったら終了
        if (!drawingObj)
        {
            // もしHPバーを描画していれば消す
            if (_hpBar)
            {
                Destroy(_hpBar);
                // 何も入っていないとする
                _hpBarObj = null;
                _hpBar = null;
            }

            return;
        }

        // HPバーの生成
        CreateHpBar(drawingObj);

        // HPバーの情報更新
        ChangeHpBarInfo();
    }

    private bool IsExist(GameObject item)
    {
        if (item.tag == "Farm")
        {
            // MEMO: 農場は破壊されているかを確認するため結果を反転して送る
            return !item.GetComponent<Farm>().IsBreak;
        }
        else if (item.tag == "Enemy")
        {
            return item.GetComponent<EnemyBase>().IsExist;
        }

        return false;
    }

    private void CreateHpBar(GameObject item)
    {
        // オブジェクトが違う場合は変更
        if (_hpBarObj != item)
        {
            _hpBarObj = item;
        }
        
        // HPバーが生成されていなければ作成
        if (!_hpBar)
        {
            // TODO: 作成
            _hpBar = Instantiate(_barPrefab, _canvas.transform);
        }
    }

    /// <summary>
    /// Hpバーの情報を変更
    /// </summary>
    private void ChangeHpBarInfo()
    {
        int nowHp = 0;
        int deltaHp = 0;
        int maxHp = 100;

        Text text = _hpBar.transform.GetChild(2).GetComponent<Text>();
        // 農場の場合
        if (_hpBarObj.tag == "Farm")
        {
            Farm script = _hpBarObj.GetComponent<Farm>();
            nowHp = script.Hp;
            deltaHp = script.DeltaHp;
            maxHp = script.MaxHp;

            text.text = "のうじょう：" + nowHp + " / " + maxHp;
        }
        // 敵の場合
        else if (_hpBarObj.tag == "Enemy")
        {
            // TODO: 敵の場合も上と同じ処理になるようにする
            EnemyBase script = _hpBarObj.GetComponent<EnemyBase>();
            nowHp = script.Hp;
            deltaHp = script.DeltaHp;
            maxHp = script.MaxHp;

            var name = _hpBarObj.name;
            if (name == "E_0(Clone)")
            {
                text.text = "さる";
            }
            else if (name == "E_1(Clone)")
            {
                text.text = "やもり";
            }
            else if (name == "E_2(Clone)")
            {
                text.text = "ねずみ";
            }
            else if (name == "E_3(Clone)")
            {
                text.text = "しか";
            }
            else if (name == "E_4(Clone)")
            {
                text.text = "へび";
            }
            else if (name == "E_5(Clone)")
            {
                text.text = "さかな";
            }
            else if (name == "E_6(Clone)")
            {
                text.text = "すずめ";
            }
            else if (name == "E_7(Clone)")
            {
                text.text = "いか";
            }

            text.text += "：" + nowHp + " / " + maxHp;;
        }

        /* スライダーの変更 */
        Slider hpSlider = _hpBar.transform.GetChild(1).GetComponent<Slider>();
        Slider deltahpSlider = _hpBar.transform.GetChild(0).GetComponent<Slider>();
        // 割合
        float nowRate = (float)nowHp / (float)maxHp;
        float deltaRate = (float)deltaHp / (float)maxHp;
        // 変更
        hpSlider.value = nowRate;
        deltahpSlider.value = deltaRate;
    }

    /// <summary>
    /// 値を制限する関数
    /// </summary>
    /// <param name="val">制限したい値</param>
    /// <param name="min">最小値</param>
    /// <param name="max">最大値</param>
    /// <returns>制限した値</returns>
    private float LimitValue(float val, float min, float max)
    {
        if (val > 0.0f)
        {
            // 正の場合
            val = (val - min) / (max - min);
            val = Mathf.Min(1.0f, Mathf.Max(0.0f, val));
        }
        else
        {
            // 負の場合
            val = (val + min) / (max - min);
            val = Mathf.Min(0.0f, Mathf.Max(-1.0f, val));
        }

        return val;
    }
}
