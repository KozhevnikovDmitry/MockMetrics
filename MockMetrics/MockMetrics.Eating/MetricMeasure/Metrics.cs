namespace MockMetrics.Eating.MetricMeasure
{
    public class Metrics
    {
        private Metrics()
        {
            
        }

        public static Metrics Create()
        {
            return new Metrics();
        }

        public static Metrics Create(Scope scope)
        {
            return new Metrics
            {
                Scope = scope
            };
        }

        public static Metrics Create(Call call)
        {
            return new Metrics
            {
                Call = call
            };
        }

        public static Metrics Create(Variable variable)
        {
            return new Metrics
            {
                Variable = variable
            };
        }

        public static Metrics Create(Scope scope, Call call)
        {
            return new Metrics
            {
                Scope = scope,
                Call = call
            };
        }

        public static Metrics Create(Scope scope, Variable variable)
        {
            return new Metrics
            {
                Scope = scope,
                Variable = variable
            };
        }

        public static Metrics Create(Scope scope, Call call, Variable variable)
        {
            return new Metrics
            {
                Scope = scope,
                Variable = variable,
                Call = call
            };
        }

        public static Metrics Create(Metrics metrics, Scope scope, Call call, Variable variable)
        {
            var result = new Metrics
            {
                Scope = scope,
                Variable = variable,
                Call = call
            };

            if (scope != Scope.None)
                result.Scope = scope;

            if (variable != Variable.None)
                result.Variable = variable;

            if (call != Call.None)
                result.Call = call;

            return result;
        }

        

        public Scope Scope { get; set; }

        public Variable Variable { get; set; }

        public Call Call { get; set; }

        public bool IsCallMetrics
        {
            get
            {
                return Call != Call.None;
            }
        }
    }
}