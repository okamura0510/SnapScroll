using UnityEngine;
using System;

namespace SnapScroll
{
    /// <summary>
    /// アンカーポジション移動Tween
    /// </summary>
    [Serializable]
    public class TweenAnchorPosition
    {
        /// <summary>
        /// 動作フラグ
        /// </summary>
        [SerializeField] bool isRunning;
        /// <summary>
        /// 動作時間
        /// </summary>
        [SerializeField] float time;
        /// <summary>
        /// RectTransform
        /// </summary>
        [SerializeField] RectTransform transform;
        /// <summary>
        /// 移動元ポジション
        /// </summary>
        [SerializeField] Vector2 from;
        /// <summary>
        /// 移動先ポジション
        /// </summary>
        [SerializeField] Vector2 to;
        /// <summary>
        /// 移動時間
        /// </summary>
        [SerializeField] float duration = 0.5f;
        /// <summary>
        /// 遅延時間
        /// </summary>
        [SerializeField] float delay;
        /// <summary>
        /// イージング
        /// </summary>
        [SerializeField] Ease ease = Ease.OutQuart;
        /// <summary>
        /// ループタイプ
        /// </summary>
        [SerializeField] Loop loop = Loop.None;

        /// <summary>
        /// 動作フラグ
        /// </summary>
        public bool IsRunning { get { return isRunning; } }
        /// <summary>
        /// 移動時間
        /// </summary>
        public float Duration { get { return duration; } set { duration = value; } }
        /// <summary>
        /// 遅延時間
        /// </summary>
        public float Delay { get { return delay; } set { delay = value; } }
        /// <summary>
        /// イージング
        /// </summary>
        public Ease Ease { get { return ease; } set { ease = value; } }
        /// <summary>
        /// ループタイプ
        /// </summary>
        public Loop Loop { get { return loop; } set { loop = value; } }
        
        /// <summary>
        /// 移動開始
        /// </summary>
        /// <param name="transform">RectTransform</param>
        /// <param name="to">移動先ポジション</param>
        public void Run(RectTransform transform, Vector2 to)
        {
            isRunning      = true;
            time           = 0;
            this.transform = transform;
            from           = transform.anchoredPosition;
            this.to        = to;
        }

        /// <summary>
        /// 移動更新
        /// </summary>
        public void Update()
        {
            if(!isRunning) return;
            
            if(delay > 0)
            {
                delay -= Time.deltaTime;
            }
            else
            {
                time += Time.deltaTime;
                if(time >= duration)
                {
                    transform.anchoredPosition = from + (to - from);
                    if(loop == Loop.None)
                    {
                        Stop();
                    }
                    else if(loop == Loop.Loop)
                    {
                        transform.anchoredPosition = from;
                        Run(transform, to);
                    }
                    else if(loop == Loop.PingPong)
                    {
                        var temp = from;
                        from     = to;
                        to       = temp;
                        Run(transform, to);
                    }
                }
                else
                {
                    var progress               = EvaluateProgress(time, ease, duration);
                    transform.anchoredPosition = from + (to - from) * progress;
                }
            }
        }
        
        /// <summary>
        /// 移動停止
        /// </summary>
        public void Stop()
        {
            isRunning = false;
        }
        
        /// <summary>
        /// 移動進捗を計算
        /// </summary>
        /// <param name="t">動作時間</param>
        /// <param name="ease">イージング</param>
        /// <param name="duration">移動時間</param>
        /// <returns>移動進捗(0〜1)</returns>
        static float EvaluateProgress(float t, Ease ease, float duration)
        {
            switch(ease)
            {
                case Ease.Linear:
                    return t / duration;
                case Ease.InQuad:
                    return (t /= duration) * t;
                case Ease.OutQuad:
                    return -(t /= duration) * (t - 2);
                case Ease.InOutQuad:
                    if((t /= duration * 0.5f) < 1) return 0.5f * t * t;
                    return -0.5f * ((--t) * (t - 2) - 1);
                case Ease.InCubic:
                    return (t /= duration) * t * t;
                case Ease.OutCubic:
                    return ((t = t / duration - 1) * t * t + 1);
                case Ease.InOutCubic:
                    if((t /= duration * 0.5f) < 1) return 0.5f * t * t * t;
                    return 0.5f * ((t -= 2) * t * t + 2);
                case Ease.InQuart:
                    return (t /= duration) * t * t * t;
                case Ease.OutQuart:
                    return -((t = t / duration - 1) * t * t * t - 1);
                case Ease.InOutQuart:
                    if((t /= duration * 0.5f) < 1) return 0.5f * t * t * t * t;
                    return -0.5f * ((t -= 2) * t * t * t - 2);
                case Ease.InQuint:
                    return (t /= duration) * t * t * t * t;
                case Ease.OutQuint:
                    return ((t = t / duration - 1) * t * t * t * t + 1);
                case Ease.InOutQuint:
                    if((t /= duration * 0.5f) < 1) return 0.5f * t * t * t * t * t;
                    return 0.5f * ((t -= 2) * t * t * t * t + 2);
                case Ease.InSine:
                    return -Mathf.Cos(t / duration * Mathf.PI * 0.5f) + 1;
                case Ease.OutSine:
                    return Mathf.Sin(t / duration * Mathf.PI * 0.5f);
                case Ease.InOutSine:
                    return -0.5f * (Mathf.Cos(Mathf.PI * t / duration) - 1);
                case Ease.InExpo:
                    if(Mathf.Abs(t) < Mathf.Epsilon) return 0;
                    return Mathf.Pow(2, 10 * (t / duration - 1));
                case Ease.OutExpo:
                    if(Mathf.Abs(t - duration) < Mathf.Epsilon) return 1;
                    return (-Mathf.Pow(2, -10 * t / duration) + 1);
                case Ease.InOutExpo:
                    if(Mathf.Abs(t) < Mathf.Epsilon) return 0;
                    if(Mathf.Abs(t - duration) < Mathf.Epsilon) return 1;
                    if((t /= duration * 0.5f) < 1) return 0.5f * Mathf.Pow(2, 10 * (t - 1));
                    return 0.5f * (-Mathf.Pow(2, -10 * --t) + 2);
                case Ease.InCirc:
                    return -(Mathf.Sqrt(1 - (t /= duration) * t) - 1);
                case Ease.OutCirc:
                    return Mathf.Sqrt(1 - (t = t / duration - 1) * t);
                case Ease.InOutCirc:
                    if((t /= duration * 0.5f) < 1) return -0.5f * (Mathf.Sqrt(1 - t * t) - 1);
                    return 0.5f * (Mathf.Sqrt(1 - (t -= 2) * t) + 1);
                default:
                    return 0;
            }
        }
    }
    
    /// <summary>
    /// イージング
    /// </summary>
    public enum Ease
    {
        Linear,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InSine,
        OutSine,
        InOutSine,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc
    }

    /// <summary>
    /// ループタイプ
    /// </summary>
    public enum Loop
    {
        None,
        Loop,
        PingPong
    }
}