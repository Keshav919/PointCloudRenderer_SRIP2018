﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStructures;
using CloudData;
using System.Threading;
using UnityEngine;

namespace Loading {
    /// <summary>
    /// The Loading Thread of the V2-Rendering-System (see Bachelor Thesis chapter 3.2.6 "The Loading Thread").
    /// Responsible for loading the point data.
    /// </summary>
    class V2LoadingThread {

        private ThreadSafeQueue<Node> loadingQueue;
        private bool running = true;
        private V2Cache cache;
        public static List<GameObject> holders;
        
        public V2LoadingThread(V2Cache cache, List<GameObject> container) {
            loadingQueue = new ThreadSafeQueue<Node>();
            this.cache = cache;
            holders = container;
        }

        public void Start() {
            new Thread(Run).Start();
            
        }

        private void Run() {
            try
            {
                while (running)
                {
                    Node n;
                    if (loadingQueue.TryDequeue(out n))
                    {
                        Monitor.Enter(n);
                        if (!n.HasPointsToRender() && !n.HasGameObjects())
                        {
                            Monitor.Exit(n);
                            
                            CloudLoader.LoadPointsForNode(n);
                            cache.Insert(n);
                        }
                        else
                        {
                            Monitor.Exit(n);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            Debug.Log("Loading Thread stopped");
        }

        public void Stop() {
            running = false;
        }

        /// <summary>
        /// Schedules the given node for loading.
        /// </summary>
        /// <param name="node">not null</param>
        public void ScheduleForLoading(Node node) {
            loadingQueue.Enqueue(node);
        }

    }
}
