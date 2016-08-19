using System;
using System.Reflection;

namespace DiceBot.RandomNumberGenerators
{
	public class RandomWithNotify : Random
    {
        public event EventHandler Sampled;

        protected virtual void OnSampled()
        {
            Sampled?.Invoke(this, EventArgs.Empty);
        }

		protected override double Sample()
		{
			double result = base.Sample();
			OnSampled();

			return result;
		}

		/// <summary>
		/// Creates a statewise copy of the current instance.
		/// </summary>
		/// <returns>The copy.</returns>
		public RandomWithNotify Clone()
		{
			var clone = new RandomWithNotify();

			var field = typeof(Random).GetField("inext", BindingFlags.Instance | BindingFlags.NonPublic);
			field.SetValue(clone, field.GetValue(this));

			field = typeof(Random).GetField("inextp", BindingFlags.Instance | BindingFlags.NonPublic);
			field.SetValue(clone, field.GetValue(this));

			field = typeof(Random).GetField("SeedArray", BindingFlags.Instance | BindingFlags.NonPublic);
			int[] arr = (int[])field.GetValue(this);
			field.SetValue(clone, arr.Clone());

			return clone;
		}
	}
}
