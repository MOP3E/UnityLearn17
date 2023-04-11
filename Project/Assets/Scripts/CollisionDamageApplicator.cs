using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class CollisionDamageApplicator : MonoBehaviour
    {
        public static string IgnoreTag = "WorldBoundary";

        /// <summary>
        /// Базовый урон.
        /// </summary>
        [SerializeField] private float _baseDamage;

        /// <summary>
        /// Модификатор урона от скорости.
        /// </summary>
        [SerializeField] private float _velocityDamageModifier;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if(col.transform.CompareTag(IgnoreTag)) return;

            Destructible destructible = transform.root.GetComponent<Destructible>();
            if(destructible == null) return;

            destructible.Hit((int)(_baseDamage + _velocityDamageModifier * col.relativeVelocity.magnitude));
        }
    }
}
