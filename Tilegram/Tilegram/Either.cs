using System;

namespace Tilegram
{
    public class Either<TLeft, TRight>
    {
        public TLeft LeftValue { get; set; }
        public TRight RightValue { get; set; }
        public bool Success { get; set; }

        private Either(TLeft left, TRight right, bool success)
        {
            LeftValue = left;
            RightValue = right;
            Success = success;
        }

        public static Either<TLeft, TRight> Left(TLeft left)
        {
            return new Either<TLeft, TRight>(left, default(TRight), false);
        }

        public static Either<TLeft, TRight> Right(TRight right)
        {
            return new Either<TLeft, TRight>(default(TLeft), right, true);
        }

        public void Match(Action<TLeft> left, Action<TRight> right)
        {
            if (Success)
                right?.Invoke(RightValue);
            else
                left?.Invoke(LeftValue);
        }

        public Back Match<Back>(Func<TLeft, Back> left, Func<TRight, Back> right)
        {
            if (Success)
                return right.Invoke(RightValue);

            return left.Invoke(LeftValue);
        }
    }
}
