﻿namespace dotless.Core.Parser.Tree
{
    using System.Linq;
    using dotless.Core.Parser.Infrastructure;
    using dotless.Core.Parser.Infrastructure.Nodes;

    class KeyFrame : Ruleset
    {
        public string Identifier { get; set; }

        public KeyFrame(string identifier, NodeList rules)
        {
            Identifier = identifier;
            Rules = rules;
        }

        public override Node Evaluate(Env env)
        {
            env.Frames.Push(this);

            Rules = new NodeList(Rules.Select(r => r.Evaluate(env))).ReducedFrom<NodeList>(Rules);

            env.Frames.Pop();
            return this;
        }

        protected override void AppendCSS(Env env, Context context)
        {
            env.Output.Append(Identifier);

            // Append pre comments as we out put each rule ourselves
            if (Rules.PreComments)
            {
                env.Output.Append(Rules.PreComments);
            }

            AppendRules(env);
        }
    }
}
