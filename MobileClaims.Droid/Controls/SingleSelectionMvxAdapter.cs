using Android.Content;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MobileClaims.Droid
{
    class SingleSelectionMvxAdapter : MvxAdapter
    {
        private readonly Context _context;
        private readonly IMvxAndroidBindingContext _bindingContext;

        public SingleSelectionMvxAdapter(Context c) : this(c, MvxAndroidBindingContextHelpers.Current())
        {
        }

        public SingleSelectionMvxAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
            _context = context;
            _bindingContext = bindingContext;
        }

        protected override IMvxListItemView CreateBindableView(object dataContext, ViewGroup parent, int templateId)
        {
            return new SingleSelectionListItemView(_context, _bindingContext.LayoutInflaterHolder, dataContext, parent, templateId);
        }
    }
}