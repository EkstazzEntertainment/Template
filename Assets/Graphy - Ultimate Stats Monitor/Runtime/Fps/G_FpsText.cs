/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            22-Nov-17
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;
using Tayx.Graphy.Utils.NumString;

namespace Tayx.Graphy.Fps
{
    public class G_FpsText : MonoBehaviour
    {
        #region Variables -> Serialized Private

        [SerializeField] private    Text            m_fpsText               = null;
        [SerializeField] private    Text            m_msText                = null;

        [SerializeField] private    Text            m_avgFpsText            = null;
        [SerializeField] private    Text            m_onePercentFpsText     = null;
        [SerializeField] private    Text            m_zero1PercentFpsText   = null;

        #endregion

        #region Variables -> Private

        private                     GraphyManager   m_graphyManager     = null;

        private                     G_FpsMonitor    m_fpsMonitor        = null;

        private                     int             m_updateRate        = 4;  // 4 updates per sec.

        private                     float           m_deltaTime         = 0f;

        private                     float           m_fps               = 0f;

        private                     float           m_ms               = 0f;

        private const               string          m_msStringFormat    = "0.0";

        #endregion

        #region Methods -> Unity Callbacks

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            m_deltaTime += Time.unscaledDeltaTime;

            // Only update texts 'm_updateRate' times per second

            if (m_deltaTime > 1f / m_updateRate)
            {
                m_fps = m_fpsMonitor.CurrentFPS;
                m_ms = m_fpsMonitor.CurrentMs;

                // Update fps
                m_fpsText.text = Mathf.RoundToInt(m_fps).ToStringNonAlloc();
                SetFpsRelatedTextColor(m_fpsText, m_fps);

                // Update ms
                m_msText.text = m_ms.ToStringNonAlloc(m_msStringFormat);
                SetFpsRelatedTextColor(m_msText, m_fps);

                if (m_fpsMonitor.AreStatsAccumulated)
                {
                    // Update 1% fps
                    m_onePercentFpsText.text = ((int)(m_fpsMonitor._80PercentileFPS)).ToStringNonAlloc();
                    SetFpsRelatedTextColor(m_onePercentFpsText, m_fpsMonitor._80PercentileFPS);

                    // Update 0.1% fps
                    m_zero1PercentFpsText.text = ((int)(m_fpsMonitor._95PercentileFPS)).ToStringNonAlloc();
                    SetFpsRelatedTextColor(m_zero1PercentFpsText, m_fpsMonitor._95PercentileFPS);

                    // Update avg fps
                    m_avgFpsText.text = ((int)(m_fpsMonitor.AverageFPS)).ToStringNonAlloc();
                    SetFpsRelatedTextColor(m_avgFpsText, m_fpsMonitor.AverageFPS);
                }
                
                // Reset variables
                m_deltaTime = 0f;
            }
        }

        #endregion
        
        #region Methods -> Public

        public void UpdateParameters()
        {
            m_updateRate = m_graphyManager.FpsTextUpdateRate;
        }

        #endregion

        #region Methods -> Private

        /// <summary>
        /// Assigns color to a text according to their fps numeric value and
        /// the colors specified in the 3 categories (Good, Caution, Critical).
        /// </summary>
        /// 
        /// <param name="text">
        /// UI Text component to change its color
        /// </param>
        /// 
        /// <param name="fps">
        /// Numeric fps value
        /// </param>
        private void SetFpsRelatedTextColor(Text text, float fps)
        {
            if (fps > m_graphyManager.GoodFPSThreshold)
            {
                text.color = m_graphyManager.GoodFPSColor;
            }
            else if (fps > m_graphyManager.CautionFPSThreshold)
            {
                text.color = m_graphyManager.CautionFPSColor;
            }
            else
            {
                text.color = m_graphyManager.CriticalFPSColor;
            }
        }

        private void Init()
        {
            G_IntString.Init( 0, 2000 );  // Max fps expected
            G_FloatString.Init( 0, 100 ); // Max ms expected per frame

            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_fpsMonitor = GetComponent<G_FpsMonitor>();
            
            UpdateParameters();
        }

        #endregion
    }
}
