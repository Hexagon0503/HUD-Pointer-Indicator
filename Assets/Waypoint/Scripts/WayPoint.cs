using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace FormatGames
{
    public class WayPoint : MonoBehaviour
    {
        [Space(20)]

        //--------------------- PUBLIC -------------------//

        [Header("WAYPOINT REFERENCES"), Space(10)]

        public float Hide_Range;
        public Image UIWayPoint;
        public Text DistanceTxt;
        public Camera cam;
        public Transform Player;
        public Transform target;
        public Transform Arrow;
        public GameObject HideOnClamp;
        public CanvasGroup Group;
        public Vector3 offset;
        public Canvas uiCanvas;
        public RectTransform uiCanvasRect;
        #region ON CLAMP

        [Serializable] public class OnClamp : UnityEvent { }

        [Space(20)] public OnClamp onClamp;

        public OnClamp onClick
        {
            get { return onClamp; }
            set { onClamp = value; }
        }

        private void CheckClamp()
        {
            if (pos.x < minX)
            {
                // LEFT

                ClampDirection = 180;
                onClamp.Invoke();
            }
            else if (pos.x > maxX)
            {
                // RIGHT

                ClampDirection = 0;
                onClamp.Invoke();
            }
            else if (pos.y < minY)
            {
                // DOWN

                ClampDirection = 270;
                onClamp.Invoke();
            }
            else if (pos.y > maxY)
            {
                // UP

                ClampDirection = 90;
                onClamp.Invoke();
            }
            else
            {
                onClamp.Invoke();
                ClampDirection = -1;
            }
        }
        #endregion

        //--------------------- PRIVATE -------------------//

        private float ClampDirection;
        private float Distance;
        private bool isHide;
        private Vector3 pos;

        #region Screen Calculations
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;
        #endregion


        private void Start()
        {
            float uiWidth = UIWayPoint.GetPixelAdjustedRect().width;
            float canvasWidthHalf = uiCanvasRect.sizeDelta.x / 2;
            minX = -canvasWidthHalf + uiWidth;
            maxX = canvasWidthHalf - uiWidth;

            float canvasHeightHalf = uiCanvasRect.sizeDelta.y / 2;
            float uiHeight = UIWayPoint.GetPixelAdjustedRect().height;
            minY = -canvasHeightHalf + uiHeight;
            maxY = canvasHeightHalf - uiHeight;
        }

        public void Update()
        {
            switch (isHide)
            {
                case true:

                    Group.alpha = Mathf.Lerp(Group.alpha, 0, Time.deltaTime * 8);

                    break;

                case false:

                    if (Group.alpha < 0.9f) { Group.alpha = Mathf.Lerp(Group.alpha, 1, Time.deltaTime * 3); }
                    CheckClamp();

                    break;
            }
        }

        public void Clamping()
        {
            if (ClampDirection != -1)
            {
                Arrow.gameObject.SetActive(true);
                Arrow.rotation = Quaternion.Euler(0f, 0f, ClampDirection);
                HideOnClamp.SetActive(false);
            }
            else
            {
                Arrow.gameObject.SetActive(false);
                HideOnClamp.SetActive(true);
            }
        }

        private void FixedUpdate()
        {
            // Hide on player getting close

            if (Vector3.Distance(target.position, Player.position) <= Hide_Range)
            {
                isHide = true;
            }
            else
            {
                isHide = false;
            }


            if (Player == null) { return; }

            pos = uiCanvas.WorldToCanvas(target.position + offset, Camera.main);

            // Clamp Icon To the screen

            Vector2 clamp_screen = pos;
            clamp_screen.x = Mathf.Clamp(clamp_screen.x, minX, maxX);
            clamp_screen.y = Mathf.Clamp(clamp_screen.y, minY, maxY);
            UIWayPoint.rectTransform.anchoredPosition3D = Vector3.Lerp(UIWayPoint.rectTransform.anchoredPosition3D, clamp_screen, Time.deltaTime * 40);

            // Display distance
            Distance = Vector3.Distance(target.position, Player.position);
            DistanceTxt.text = Distance.ToString("0") + "m";
        }
    }
}