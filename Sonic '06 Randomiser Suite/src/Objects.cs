using HedgeLib;
using HedgeLib.Sets;

namespace Sonic_06_Randomiser_Suite
{
    class Objects
    {
        /// <summary>
        /// Creates a jumppanel object with the specified parameters
        /// </summary>
        public static SetObject jumppanel(Vector3 position, Quaternion rotation, float pitch, float velocity, float time, uint target, uint objectID) {
            SetObject jumppanel = new SetObject() {
                ObjectType = "jumppanel",
                ObjectID = objectID,
                ObjectName = $"jumppanel{objectID}",
                DrawDistance = 0,
                UnknownBytes = new byte[] { 64, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 }
            };

            jumppanel.Transform.Position = position;
            jumppanel.Transform.Rotation = rotation;

            jumppanel.Parameters.Add(new SetObjectParam(typeof(float), pitch));
            jumppanel.Parameters.Add(new SetObjectParam(typeof(float), velocity));
            jumppanel.Parameters.Add(new SetObjectParam(typeof(float), time));
            jumppanel.Parameters.Add(new SetObjectParam(typeof(uint), target));

            return jumppanel;
        }

        /// <summary>
        /// Creates a spring object with the specified parameters
        /// </summary>
        public static SetObject spring(Vector3 position, Quaternion rotation, float velocity, float time, uint target, int globalflag, uint objectID, bool town) {
            SetObject spring = new SetObject() {
                ObjectType = "spring",
                ObjectID = objectID,
                ObjectName = $"spring{objectID}",
                DrawDistance = 0,
                UnknownBytes = new byte[] { 64, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 }
            };

            if (town) spring.ObjectType = "spring_twn";
            if (town) spring.ObjectName = $"spring_twn{objectID}";

            spring.Transform.Position = position;
            spring.Transform.Rotation = rotation;

            spring.Parameters.Add(new SetObjectParam(typeof(float), velocity));
            spring.Parameters.Add(new SetObjectParam(typeof(float), time));
            spring.Parameters.Add(new SetObjectParam(typeof(uint), target));
            if (town) spring.Parameters.Add(new SetObjectParam(typeof(int), globalflag));

            return spring;
        }

        /// <summary>
        /// Creates a pointsample object with the specified parameters
        /// </summary>
        public static SetObject pointsample(Vector3 position, Quaternion rotation, int content, uint objectID) {
            SetObject pointsample = new SetObject() {
                ObjectType = "pointsample",
                ObjectID = objectID,
                ObjectName = $"pointsample{objectID}",
                DrawDistance = 0,
                UnknownBytes = new byte[] { 64, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 }
            };

            pointsample.Transform.Position = position;
            pointsample.Transform.Rotation = rotation;

            pointsample.Parameters.Add(new SetObjectParam(typeof(int), content));

            return pointsample;
        }
    }
}
