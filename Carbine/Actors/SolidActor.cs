using SFML.Graphics;
using SFML.System;
using System;
using Carbine.Collision;

namespace Carbine.Actors
{
    /// <summary>
    /// An abstract class to inherit from for actors with collision
    /// </summary>
    public abstract class SolidActor : Actor, ICollidable
    {
        private const int COLLISION_RESULTS_SIZE = 8;

        /// <summary>
        /// 
        /// </summary>
        protected AABB aabb;
        /// <summary>
        /// The collision manager
        /// </summary>
        protected CollisionManager collisionManager;
        /// <summary>
        /// The results of any collision
        /// </summary>
        private readonly ICollidable[] collisionResults;
        /// <summary>
        /// Debug vertices
        /// </summary>
        private VertexArray debugVerts;
        /// <summary>
        /// Collision types to ignore
        /// </summary>
        protected Type[] ignoreCollisionTypes;
        /// <summary>
        /// Is this solid actor able to move?
        /// </summary>
        protected bool isMovementLocked;
        /// <summary>
        /// Is this object able to collide with things
        /// </summary>
        protected bool isSolid;
        /// <summary>
        /// The position of the actor in the last frame
        /// </summary>
        protected Vector2f lastPosition;
        /// <summary>
        /// The mesh of the actor
        /// </summary>
        protected Mesh mesh;
        /// <summary>
        /// The temporary movement vector2
        /// </summary>
        private Vector2f moveTemp;

        public SolidActor(CollisionManager colman)
        {
            collisionManager = colman;
            ignoreCollisionTypes = null;
            isSolid = true;
            collisionResults = new ICollidable[8];
            if (collisionManager != null)
            {
                collisionManager.Add(this);
            }
        }


        public Vector2f LastPosition => lastPosition;

        public virtual bool MovementLocked
        {
            get => isMovementLocked;
            set => isMovementLocked = value;
        }

        public AABB AABB => aabb;

        public Mesh Mesh => mesh;

        public bool Solid
        {
            get => isSolid;
            set => isSolid = value;
        }

        public VertexArray DebugVerts => GetDebugVerts();

        public virtual void Collision(CollisionContext context)
        {
        }

        private VertexArray GetDebugVerts()
        {
            if (debugVerts == null)
            {
                VertexArray vertexArray = new VertexArray(PrimitiveType.LinesStrip, (uint)(mesh.Vertices.Count + 1));

                for (int i = 0; i < mesh.Vertices.Count; i++)
                {
                    vertexArray[(uint)i] = new Vertex(mesh.Vertices[i], Color.Green);
                }

                vertexArray[(uint)mesh.Vertices.Count] = new Vertex(mesh.Vertices[0], Color.Green);
                debugVerts = vertexArray;
            }

            return debugVerts;
        }

        protected virtual void HandleCollision(ICollidable[] collisionObjects)
        {

            //   ;
        }

        public override void Update()
        {
            lastPosition = position;
            if (
                //if our collision manager exists
                collisionManager != null &&
                //and we cannot move next
                !collisionManager.PlaceFree(this, position, collisionResults, ignoreCollisionTypes)
            )
            {
                //collision
                HandleCollision(collisionResults);
            }
            else
            {
                if (velocity.X != 0f && !isMovementLocked)
                {
                    moveTemp = new Vector2f(position.X + velocity.X, position.Y);
                    bool flag = collisionManager == null ||
                                collisionManager.PlaceFree(this, moveTemp, collisionResults, ignoreCollisionTypes);
                    if (flag)// && collisionManager.ObjectsAtPosition(position).Count() < 0)
                    {
                        position = moveTemp;
                    }
                    else
                    {
                        velocity.X = 0f;
                        HandleCollision(collisionResults);
                    }
                }

                if (Velocity.Y != 0f && !isMovementLocked)
                {
                    moveTemp = new Vector2f(position.X, position.Y + velocity.Y);
                    bool flag = collisionManager == null ||
                                collisionManager.PlaceFree(this, moveTemp, collisionResults, ignoreCollisionTypes);
                    if (flag)
                    {
                        position = moveTemp;
                    }
                    else
                    {
                        velocity.Y = 0f;
                        HandleCollision(collisionResults);
                    }
                }
            }

            if (!lastPosition.Equals(position) && collisionManager != null)
            {


                collisionManager.Update(this, lastPosition, position);
            }
        }
    }
}