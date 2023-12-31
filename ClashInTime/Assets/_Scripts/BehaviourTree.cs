using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NodeState state;
        protected List<Node> children = new List<Node>();

        public Node parent;

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach(Node child in children)
                _Attach(child);
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        private Dictionary<string, object> _dataContext = 
            new Dictionary<string, object>();
        
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if(_dataContext.TryGetValue(key, out value))
                return value;
            
            Node node = parent;

            while(node != null)
            {
                value = node.GetData(key);
                if(value != null)
                    return value;
                node = node.parent;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if(_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;

            while(node != null)
            {
                bool cleared = node.ClearData(key);

                if(cleared)
                    return true;
                
                node = node.parent;
            }

            return false;
        }

        public Node GetRoot()
        {
            Node node = parent;

            while (node.parent != null)
            {
                node = node.parent;
            }
            return node;
        }
    }

    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;
        protected Character _char = null;

        protected BasicAnimationController _charAnim = null;
        protected GameController _gameCon = null;

        protected void Start()
        {
            _char = transform.GetComponent<Character>();
            _charAnim = transform.GetComponent<BasicAnimationController>();
            _gameCon = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            _root = SetupTree();
        }

        private void Update()
        {
            if(_root != null)
                _root.Evaluate();
        }

        protected abstract Node SetupTree();

        public void GetRoot(Node root)
        {
            root = _root;
        }
    }

    public class Sequence : Node
    {
        public Sequence() : base() {}
        public Sequence(List<Node> children) : base(children) {}

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach(Node node in children)
            {
                switch(node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = anyChildIsRunning? NodeState.RUNNING: NodeState.SUCCESS;
            return state;
        }
    }

    public class Selector: Node
    {
        public Selector() : base() {}
        public Selector(List<Node> children) : base(children) {}

        public override NodeState Evaluate()
        {
            foreach(Node node in children)
            {
                switch(node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}
