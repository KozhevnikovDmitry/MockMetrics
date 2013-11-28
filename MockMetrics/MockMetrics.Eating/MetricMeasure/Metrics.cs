namespace MockMetrics.Eating.MetricMeasure
{
    public struct Metrics
    {
        public static Metrics Create()
        {
            return new Metrics();
        }

        public static Metrics Create(Call call)
        {
            return new Metrics
            {
                Call = call
            };
        }

        public static Metrics Create(VarType varType = VarType.None, Call call = Call.None)
        {
            return new Metrics
            {
                VarType = varType,
                Call = call
            };
        }

        public static Metrics Create(Aim aim = Aim.None, VarType varType = VarType.None, Call call = Call.None)
        {
            return new Metrics
            {
                VarType = varType,
                Aim = aim,
                Call = call
            };
        }

        public static Metrics Create(VarType varType, Aim aim)
        {
            return new Metrics
            {
                VarType = varType,
                Aim = aim
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

        public static Metrics Create(Scope scope = Scope.None, VarType varType = VarType.None, Aim aim = Aim.None, Call call = Call.None)
        {
            return new Metrics
            {
                VarType = varType,
                Aim = aim,
                Scope = scope,
                Call = call
            };
        }

        public static Metrics Create(Metrics metrics, Scope scope = Scope.None, VarType varType = VarType.None, Aim aim = Aim.None, Call call = Call.None)
        {
            var result = new Metrics()
            {
                Scope = metrics.Scope,
                Aim = metrics.Aim,
                VarType = metrics.VarType,
                Call = metrics.Call
            };

            if (scope != Scope.None)
                result.Scope = scope;

            if (aim != Aim.None)
                result.Aim = aim;

            if (varType != VarType.None)
                result.VarType = varType;

            if (call != Call.None)
                result.Call = call;

            return result;
        }

        public Scope Scope { get; set; }
        
        public Aim Aim { get; set; }

        public VarType VarType { get; set; }

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