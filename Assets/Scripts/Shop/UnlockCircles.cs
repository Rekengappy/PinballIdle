﻿using Controllers;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class UnlockCircles : MonoBehaviour
    {
        public static UpgradeCircle upgrade;

        private bool _isOpen = true;

        private bool _isMax;
        private static readonly int[] maxUpgrade = {12, 14, 12, 15, 16, 14, 16, 16, 14};

        [SerializeField]
        private Text _costText;

        [SerializeField]
        private Text _count;

        [SerializeField]
        private Image _imageCost;

        private const int START_COST = 10;
        private const int MULTI_COST = 10;

        private readonly int[] _cost = {10, 10, 10, 10, 10, 10, 10, 10, 10};

        private void Awake()
        {
            MenuController.openMenu[MenuController.Shops.UpgradeFields] += UpdateText;
        }

        private void Start()
        {
            if (maxUpgrade[0] != GameManager.instance.fields[0].circles.Length - 1)
            {
                Debug.LogError(
                    $"Max unlock circle ({maxUpgrade}) not equal current circle on field ({GameManager.instance.fields[0].circles.Length - 1})");
            }

            for (int _field = 0; _field < FieldManager.fields.isOpen.Length; _field++)
            {
                if (!FieldManager.fields.isOpen[_field]) continue;
                _cost[_field] = START_COST + MULTI_COST * upgrade.upgrades[_field];
                for (int _i = 0; _i < upgrade.upgrades[_field]; _i++)
                {
                    OpenCircle(_field, _i);
                }
            }
        }

        private void Update()
        {
            if (MenuController.currentMenu != 1) return;
            if (_isMax)
                return;
            switch (_isOpen)
            {
                case true when PlayerDataController.PointSum < _cost[FieldManager.currentField]:
                    _isOpen = false;
                    _imageCost.raycastTarget = false;
                    _imageCost.sprite = GameManager.instance._lockedSprite;
                    GameManager.TextDown(_costText.transform.parent.gameObject);
                    break;
                case false when PlayerDataController.PointSum >= _cost[FieldManager.currentField]:
                    _isOpen = true;
                    _imageCost.sprite = GameManager.instance._unlockedSprite;
                    _imageCost.raycastTarget = true;
                    GameManager.TextUp(_costText.transform.parent.gameObject);
                    break;
            }
        }

        private static void OpenCircle(int fieldNumber, int circle)
        {
            if (circle < GameManager.instance.fields[fieldNumber].circles.Length - 1)
                GameManager.instance.fields[fieldNumber].circles[circle].SetActive(true);
            if (circle == maxUpgrade[fieldNumber] - 1)
            {
                GameManager.instance.fields[fieldNumber].MakeTriple();
                Debug.Log("MakeTriple");
            }
        }

        public void BuyCircle()
        {
            if (PlayerDataController.PointSum < _cost[FieldManager.currentField]) return;
            PlayerDataController.PointSum -= _cost[FieldManager.currentField];
            Statistics.stats.pointSpent += _cost[FieldManager.currentField];
            OpenCircle(FieldManager.currentField, upgrade.upgrades[FieldManager.currentField]);
            upgrade.upgrades[FieldManager.currentField]++;
            _cost[FieldManager.currentField] = START_COST + MULTI_COST * upgrade.upgrades[FieldManager.currentField];
            UpdateText();
        }

        private void UpdateText()
        {
            if (upgrade.upgrades[FieldManager.currentField] >= maxUpgrade[FieldManager.currentField] && _isMax)
                return;
            if (upgrade.upgrades[FieldManager.currentField] >= maxUpgrade[FieldManager.currentField])
            {
                _isMax = true;
                _costText.text = "MAX";
                _count.text = (maxUpgrade[FieldManager.currentField] + 1).ToString();
                if (!_isOpen) return;
                GameManager.TextDown(_costText.transform.parent.gameObject);
                _imageCost.sprite = GameManager.instance._lockedSprite;
                _imageCost.raycastTarget = false;
            }
            else
            {
                if (_isMax)
                {
                    _isMax = false;
                    GameManager.TextUp(_costText.transform.parent.gameObject);
                    _imageCost.sprite = GameManager.instance._unlockedSprite;
                    _imageCost.raycastTarget = true;
                }

                _costText.text = _cost[FieldManager.currentField].ToString();
                _count.text = (upgrade.upgrades[FieldManager.currentField] + 1) + "→" +
                              (upgrade.upgrades[FieldManager.currentField] + 2);
            }
        }
    }


    public class UpgradeCircle
    {
        public int[] upgrades;

        public UpgradeCircle()
        {
            upgrades = new int[9];
            upgrades[0] = 6;
        }
    }
}