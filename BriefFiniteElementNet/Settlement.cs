﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace BriefFiniteElementNet
{
    [Serializable]
    public struct Settlement : IEquatable<Settlement>, ISerializable
    {
        private LoadCase loadCase;
        private Displacement displacement;

        public LoadCase LoadCase 
        {
            get         {            return loadCase;}
            set { loadCase = value; }
        }

        public Displacement Displacement
        {
            get { return displacement; }
            set { displacement = value; }
        }


        #region Equality Compairer

        public bool Equals(Settlement other)
        {
            if (!this.displacement.Equals(other.displacement))
                return false;

            if (!this.loadCase.Equals(other.loadCase))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Settlement && Equals((Settlement)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = loadCase.GetHashCode();
                hashCode = (hashCode * 397) ^ displacement.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Settlement left, Settlement right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Settlement left, Settlement right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region Serialization Stuff

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("loadCase", loadCase);
            info.AddValue("displacement", displacement);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadCase"/> struct. for ISerialable.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        private Settlement(SerializationInfo info, StreamingContext context)
        {
            loadCase = info.GetValue<LoadCase>("caseName");
            displacement = info.GetValue<Displacement>("loadType");
        }

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="LoadCase"/> struct.
        /// </summary>
        /// <param name="loadCase">load case of settlement.</param>
        /// <param name="displacement">amount of settlement.</param>
        public Settlement(LoadCase loadCase, Displacement displacement)
        {
            this.loadCase = loadCase;
            this.displacement = displacement;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadCase"/> struct.
        /// </summary>
        /// <param name="displacement">amount of settlement.</param>
        public Settlement(Displacement displacement) : this(LoadCase.DefaultLoadCase, displacement)
        {
        }

    }
}
