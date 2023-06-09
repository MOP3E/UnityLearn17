using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Скрипт игрока.
    /// </summary>
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// Количество жизней.
        /// </summary>
        [SerializeField] private int _livesCount;

        /// <summary>
        /// Космический корабль на сцене.
        /// </summary>
        [SerializeField] private SpaceShip _spaceShip;

        /// <summary>
        /// Префаб космического корабля.
        /// </summary>
        [SerializeField] private GameObject _spaceShipPrefab;

        /// <summary>
        /// Префаб взрыва.
        /// </summary>
        [SerializeField] private GameObject _explodePrefab;

        /// <summary>
        /// Контроллер камеры.
        /// </summary>
        [SerializeField] private CameraContoller _cameraContoller;

        /// <summary>
        /// Контроллер управления движением.
        /// </summary>
        [SerializeField] private MovementController _movementController;

        private void Start()
        {
            //подписка на событие уничтожения корабля игрока
            _spaceShip.Destruction += ShipDestructed;
        }

        /// <summary>
        /// Обработчик события разрушения корабля игрока.
        /// </summary>
        private void ShipDestructed(Vector3 position)
        {
            _livesCount--;
            if (_livesCount > 0)
            {
                StartCoroutine(Respawn(position));
            }
        }

        /// <summary>
        /// Размещение нового корабля игрока на карте.
        /// </summary>
        private IEnumerator Respawn(Vector3 position)
        {
            //создать эффект взрыва корабля
            ParticleSystem explode = Instantiate(_explodePrefab, position, Quaternion.identity).GetComponent<ParticleSystem>();
            explode.Play();
            //ждать завершения работы эффекта взрыва
            yield return new WaitForSeconds(explode.main.duration);
            //создать новый корабль игрока
            GameObject newShip = Instantiate(_spaceShipPrefab);
            //получить ссылку на корабль
            _spaceShip = newShip.GetComponent<SpaceShip>();
            //настроить камеру
            _cameraContoller.Tagret = newShip.GetComponentInChildren<CameraTarget>().transform;
            //настроить управление кораблём
            _spaceShip.Controller = _movementController;
            //подписаться на событие разрушения корабля
            _spaceShip.Destruction += ShipDestructed;

            yield return null;
        }
    }
}
