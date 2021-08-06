using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utility
{
    public static class UtMath
    {
        public static void Lerp(this float startValue, float finishValue, float speed)
        {
            startValue = Mathf.Lerp(startValue, finishValue, Time.deltaTime * speed);
        }

        public static float LerpEvenly(this float startValue, float finishValue, float speed)
        {
            var dist = Mathf.Abs(finishValue - startValue);
            var result = Mathf.Lerp(startValue, finishValue, (Time.deltaTime * speed) / dist);
            return result;
        }

        public static int RoundOff(this int startValue, int val)
        {
            var result = 0;
            var floatVal = (float)startValue / (float)val;
            var intVal = startValue / val;
            if (floatVal > intVal)
            {
                result = intVal * val + val;
                if (floatVal > (float)intVal + 0.5f)
                {
                    result = (intVal + 1) * val;
                }
            }
            else
            {
                result = intVal * val;
            }
            return result;
        }

        public static int GetPercentage(this int maximal, int percent)
        {
            var result = 0;

            if (percent > maximal)
                result = 100;
            else if (percent < ((float)maximal / 100f))
                result = 1;
            else
            {
                var max = (float)maximal;
                var per = (float)percent;
                result = (int)(per / (max / 100f));
            }

            return result;
        }

        public static float RandomizePlusMinusValue(this float floatValue, float value)
        {
            var f = UnityEngine.Random.Range(floatValue - value, floatValue + value);
            return f;
        }
        public static float RandomizePlusMinusValue(float value)
        {
            var f = UnityEngine.Random.Range(0 - value, 0 + value);
            return f;
        }
        public static int RandomizePlusMinusValue(this int intValue, int value)
        {
            var i = UnityEngine.Random.Range(intValue - value, intValue + value);
            return i;
        }
        public static int RandomizePlusMinusValue(int value)
        {
            var i = UnityEngine.Random.Range(0 - value, 0 + value);
            return i;
        }

        public static float RandomizeTimer(this float timer)
        {
            var randomizer = timer / 5f;
            return timer + UnityEngine.Random.Range(-randomizer, randomizer);
        }

        public static int GetRandomCount(this MonoBehaviour mono, int min, int max)
        {
            var x = UnityEngine.Random.Range(min, max + 1);
            return x;
        }

        public static int GetRandomCount(int min, int max)
        {
            var x = UnityEngine.Random.Range(min, max + 1);
            return x;
        }

        public static float GetRandomCount(this MonoBehaviour mono, float min, float max)
        {
            var x = UnityEngine.Random.Range(min, max);
            return x;
        }

        public static float GetRandomBetweenMass(this MonoBehaviour mono, float[] mass)
        {
            var x = UnityEngine.Random.Range(0, mass.Length);
            return mass[x];
        }

        public static T GetRandomBetweenMass<T>(this MonoBehaviour mono, T[] mass)
        {
            var x = UnityEngine.Random.Range(0, mass.Length);
            return mass[x];
        }

        public static float GetRandomCount(float min, float max)
        {
            var x = UnityEngine.Random.Range(min, max);
            return x;
        }
    }
}
