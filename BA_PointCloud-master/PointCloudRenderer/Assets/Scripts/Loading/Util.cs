﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudData;
using UnityEngine;

namespace Loading {
    /// <summary>
    /// Various help functions
    /// </summary>
    class Util {

        /// <summary>
        /// Checks whether the bounding box is inside the frustum.
        /// Actually, there is a Unity function for this, however that one can only be called from the main thread.
        /// </summary>
        public static bool InsideFrustum(BoundingBox box, Plane[] frustum) {
            bool inside;
            for (int i = 1; i < 5; i++) {
                inside = false;
                Plane plane = frustum[i];   //Ignore Far Plane, because it doesnt work because of inf values
                inside |= plane.GetSide(new Vector3((float)box.lx, (float)box.ly, (float)box.lz));
                if (inside) continue;
                inside |= plane.GetSide(new Vector3((float)box.lx, (float)box.ly, (float)box.uz));
                if (inside) continue;
                inside |= plane.GetSide(new Vector3((float)box.lx, (float)box.uy, (float)box.lz));
                if (inside) continue;
                inside |= plane.GetSide(new Vector3((float)box.lx, (float)box.uy, (float)box.uz));
                if (inside) continue;
                inside |= plane.GetSide(new Vector3((float)box.ux, (float)box.ly, (float)box.lz));
                if (inside) continue;
                inside |= plane.GetSide(new Vector3((float)box.ux, (float)box.ly, (float)box.uz));
                if (inside) continue;
                inside |= plane.GetSide(new Vector3((float)box.ux, (float)box.uy, (float)box.lz));
                if (inside) continue;
                inside |= plane.GetSide(new Vector3((float)box.ux, (float)box.uy, (float)box.uz));
                if (!inside) return false;
            }
            return true;
        }

        /// <summary>
        /// Checks whether the vector is inside the frustum.
        /// Actually, there is a Unity function for this, however that one can only be called from the main thread.
        /// </summary>
        public static bool InsideFrustum(Vector3 vec, Plane[] frustum) {
            bool inside;
            for (int i = 0; i < 5; i++) {
                Plane plane = frustum[i];
                inside = plane.GetSide(vec);
                if (!inside) return false;
            }
            return true;
        }
    }
}
