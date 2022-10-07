using System;
using System.Collections.Generic;
using System.Linq;

namespace Carbine.Actors
{
    public class ActorManager
    {
        /// <summary>
        ///     Actors in the scene
        /// </summary>
        private readonly List<Actor> actors;

        /// <summary>
        ///     Actors to add to the scene
        /// </summary>
        private readonly Stack<Actor> actorsToAdd;

        /// <summary>
        ///     Actors to remove from the scene
        /// </summary>
        private readonly Stack<Actor> actorsToRemove;

        /// <summary>
        ///     Creates a new instance of this class
        /// </summary>
        public ActorManager()
        {
            actors = new List<Actor>();
            actorsToAdd = new Stack<Actor>();
            actorsToRemove = new Stack<Actor>();
        }

        /// <summary>
        ///     Adds an actor to the stack
        /// </summary>
        /// <param name="actor">The actor to add to the stack</param>
        public void Add(Actor actor)
        {
            actorsToAdd.Push(actor);
        }

        /// <summary>
        ///     Adds an array of actors
        /// </summary>
        /// <typeparam name="T">Reduced to generics because of inheritance.</typeparam>
        /// <param name="addActors">Actors to add</param>
        public void AddAll<T>(T[] addActors) where T : Actor
        {
            foreach (T generic in addActors)
            {
                Add(generic);
            }
        }

        /// <summary>
        ///     Removes an actor by adding it to the stack of actors to remove.
        /// </summary>
        /// <param name="actor">The actor to remove</param>
        public void Remove(Actor actor)
        {
            actorsToRemove.Push(actor);
        }

        /// <summary>
        ///     Finds an actor using a predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Actor Find(Func<Actor, bool> predicate)
        {
            return actors.FirstOrDefault(predicate);
        }


        public void Step()
        {
            //our index is the amount of our actors - 1, and then go to a for loop
            for (int index = actors.Count - 1; index >= 0; --index)
            {
                //call input for the actor
                actors[index].Input();
                //call update for the actors
                actors[index].Update();
            }

            //add actors from the stack
            actors.AddRange(actorsToAdd);
            //clear the stack
            actorsToAdd.Clear();
            //while we still have a list of actors to remove
            while (actorsToRemove.Count > 0)
            {
                //get actor
                Actor actor = actorsToRemove.Pop();
                //dispose
                actor.Dispose();
                //remove
                actors.Remove(actor);
            }

            actorsToRemove.Clear();
        }

        /// <summary>
        ///     Destroys every actor
        /// </summary>
        public void ClearActors()
        {
            for (int index = 0; index < actors.Count; ++index)
            {
                actorsToRemove.Push(actors[index]);
            }
        }
    }
}