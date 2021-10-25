using System;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels.ClaimsHistory;

namespace MobileClaims.Droid
{
	public class Utility {
	
		public static void setFullListViewHeight(ListView listView) {

			IListAdapter listAdapter = listView.Adapter;
			if (listAdapter == null) {
				return;
			}

			int totalHeight = listView.PaddingTop + listView.PaddingBottom;
			for (int i = 0; i < listAdapter.Count; i++) {
				View listItem = listAdapter.GetView (i, null, listView);
                if (listItem.GetType() == typeof(ViewGroup))
                {
                    listItem.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                }
                listItem.Measure(0, 0);
                totalHeight += listItem.MeasuredHeight + listItem.PaddingBottom + listItem.PaddingTop; 
				Console.WriteLine("Item height = {0}", listItem.MeasuredHeight);
               
			}
			Console.WriteLine("Total height = {0}", totalHeight);

			ViewGroup.LayoutParams lparams = listView.LayoutParameters;
			lparams.Height = totalHeight + (listView.DividerHeight * (listView.Count - 1));
			Console.WriteLine("Total height + dividers = {0}", lparams.Height);
			listView.LayoutParameters = lparams;
            listView.RequestLayout();
		}

        public static void setFullListViewHeightCH(ListView listView, ClaimsHistoryResultsListViewModel model)
        {
           
            int totalHeight = 0;
            IListAdapter listAdapter = listView.Adapter;
            if (listAdapter == null)
            {
                return ;
            }

            totalHeight = listView.PaddingTop + listView.PaddingBottom;
            try
            {
                for (int i = 0; i < listAdapter.Count; i++)
                {
                    View listItem = listAdapter.GetView(i, null, listView);
                    if (listItem.GetType() == typeof(ViewGroup))
                    {
                        listItem.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    }



                    listItem.Measure(Android.Views.View.MeasureSpec.MakeMeasureSpec(listView.Width, MeasureSpecMode.Exactly), Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));


                    totalHeight += listItem.MeasuredHeight;
                    Console.WriteLine("Item height = {0}", listItem.MeasuredHeight);

                }
            }
            catch { }
            Console.WriteLine("Total height = {0}", totalHeight);

            ViewGroup.LayoutParams lparams = listView.LayoutParameters;
            lparams.Height = totalHeight + (listView.DividerHeight * (listView.Count - 1));
            Console.WriteLine("Total height + dividers = {0}", lparams.Height);
           // return lparams.Height;
            listView.LayoutParameters = lparams;
            listView.PostDelayed(() =>
                {
                    model.HideLoadingCommand.Execute(null);
                },500);
            
        }
        public static void setFullListViewHeightCH(ListView listView)
        {
            int totalHeight = 0;
            IListAdapter listAdapter = listView.Adapter;
            if (listAdapter == null)
            {
                return ;
            }

            totalHeight = listView.PaddingTop + listView.PaddingBottom;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                View listItem = listAdapter.GetView(i, null, listView);
                if (listItem.GetType() == typeof(ViewGroup))
                {
                    listItem.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                }

                listItem.Measure(Android.Views.View.MeasureSpec.MakeMeasureSpec(listView.Width, MeasureSpecMode.Exactly), Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));


                totalHeight += listItem.MeasuredHeight;
                Console.WriteLine("Item height = {0}", listItem.MeasuredHeight);

            }
            Console.WriteLine("Total height = {0}", totalHeight);            
            ViewGroup.LayoutParams lparams = listView.LayoutParameters;
            lparams.Height = totalHeight + (listView.DividerHeight * (listView.Count - 1));
            Console.WriteLine("Total height + dividers = {0}", lparams.Height);
            listView.LayoutParameters = lparams;

        }
        public static void setFullListViewHeightCHBnft(ListView listView)
        {
            int totalHeight = 0;
            IListAdapter listAdapter = listView.Adapter;
            if (listAdapter == null)
            {
                return;
            }

            totalHeight = listView.PaddingTop + listView.PaddingBottom;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                View listItem = listAdapter.GetView(i, null, listView);
                Console.WriteLine("Line Count = {0}", listItem.GetType());
                if (listItem.GetType() == typeof(ViewGroup))
                {
                    listItem.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                }

                listItem.Measure(Android.Views.View.MeasureSpec.MakeMeasureSpec(listView.Width, MeasureSpecMode.Exactly), Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));

                totalHeight += listItem.MeasuredHeight;// * (lineCount + 1);                             

            }
            //Added static valies to the total height as this will fix the last element cut off the screen on some small devices
            //required to fix permenantly.
            totalHeight += 60;            

            ViewGroup.LayoutParams lparams = listView.LayoutParameters;
            lparams.Height = totalHeight + (listView.DividerHeight * (listView.Count - 1));
            Console.WriteLine("Total height + dividers = {0}", lparams.Height);
            listView.LayoutParameters = lparams;

        }
        public static void setFullListViewHeightforHCSA(ListView listView)
        {

            IListAdapter listAdapter = listView.Adapter;
            if (listAdapter == null)
            {
                return;
            }

            int totalHeight = listView.PaddingTop + listView.PaddingBottom;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                View listItem = listAdapter.GetView(i, null, listView);
                if (listItem.GetType() == typeof(ViewGroup))
                {
                    listItem.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                }
                //				listItem.Measure(Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified), Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
                listItem.Measure(0, 0);
                totalHeight += listItem.MeasuredHeight + 43;
                Console.WriteLine("Item height = {0}", listItem.MeasuredHeight);
            }
            Console.WriteLine("Total height = {0}", totalHeight);

            ViewGroup.LayoutParams lparams = listView.LayoutParameters;
            lparams.Height = totalHeight + (listView.DividerHeight * (listView.Count - 1));
            Console.WriteLine("Total height + dividers = {0}", lparams.Height);
            listView.LayoutParameters = lparams;
        }

        public static void setFullListViewHeightforHCSA(ListView listView,int offset)
        {

            IListAdapter listAdapter = listView.Adapter;
            if (listAdapter == null)
            {
                return;
            }

            int totalHeight = listView.PaddingTop + listView.PaddingBottom;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                View listItem = listAdapter.GetView(i, null, listView);
                if (listItem.GetType() == typeof(ViewGroup))
                {
                    listItem.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                }
                //				listItem.Measure(Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified), Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
                listItem.Measure(0, 0);
                totalHeight += listItem.MeasuredHeight + offset;
                Console.WriteLine("Item height = {0}", listItem.MeasuredHeight);
            }
            Console.WriteLine("Total height = {0}", totalHeight);

            ViewGroup.LayoutParams lparams = listView.LayoutParameters;
            lparams.Height = totalHeight + (listView.DividerHeight * (listView.Count - 1));
            Console.WriteLine("Total height + dividers = {0}", lparams.Height);
            listView.LayoutParameters = lparams;
        }

		public static void setListViewHeightBasedOnChildren(ListView listView) {
			IListAdapter adapter = listView.Adapter;
			if (adapter == null)
				return;

			var totalHeight = listView.PaddingTop + listView.PaddingBottom;
			for (var i = 0; i < adapter.Count; i++)
			{
				View item = adapter.GetView(i, null, listView);
				if (item.GetType () == typeof(ViewGroup)) {
					item.LayoutParameters = new ViewGroup.LayoutParams (ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
				}

				TextView txtView = (TextView)item.FindViewById (Resource.Id.text1);
				int lextLength = txtView.Text.Length;
				var lineCount = Convert.ToInt32(lextLength / 20);

				item.Measure(0,0);
				totalHeight += item.MeasuredHeight * (lineCount + 1);
				Console.WriteLine ("line count = {0}", lineCount);
				Console.WriteLine("Item height = {0}", item.MeasuredHeight);
			}

			Console.WriteLine("Total height = {0}", totalHeight);

			ViewGroup.LayoutParams layoutParams = listView.LayoutParameters;
			layoutParams.Height = totalHeight + (listView.DividerHeight * (listView.Count - 1));

			Console.WriteLine("Total height + dividers = {0}", layoutParams.Height);
			listView.LayoutParameters = layoutParams;
		}

        public static void setFullListViewHeightwithEobListAsChild(ListView listView)
        {
            int totalHeight = 0;
            IListAdapter listAdapter = listView.Adapter;
            if (listAdapter == null)
            {
                return;
            }

            totalHeight = listView.PaddingTop + listView.PaddingBottom;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                View listItem = listAdapter.GetView(i, null, listView);
                if (listItem.GetType() == typeof(ViewGroup))
                {
                    listItem.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                }

                NonSelectableList EobMessages = listItem.FindViewById<NonSelectableList>(Resource.Id.eob_messages);
                int childheight = 0;
                IListAdapter childListAdapter = EobMessages.Adapter;
                if (childListAdapter == null)
                    return;
                childheight = EobMessages.PaddingTop + EobMessages.PaddingBottom;
                for (int j = 0; j < childListAdapter.Count;  j++)
                {
                    View childlistItem = childListAdapter.GetView(j, null, EobMessages);
                    if (childlistItem.GetType() == typeof(ViewGroup))
                    {
                        childlistItem.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    }

                    childlistItem.Measure(Android.Views.View.MeasureSpec.MakeMeasureSpec(listView.Width/2, MeasureSpecMode.Exactly), Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
                    childheight += childlistItem.MeasuredHeight;
                }

             


                listItem.Measure(Android.Views.View.MeasureSpec.MakeMeasureSpec(listView.Width, MeasureSpecMode.Exactly), Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));


                totalHeight += listItem.MeasuredHeight + childheight;
                Console.WriteLine("Item height = {0}", listItem.MeasuredHeight);

            }
            Console.WriteLine("Total height = {0}", totalHeight);

            ViewGroup.LayoutParams lparams = listView.LayoutParameters;
            lparams.Height = totalHeight + (listView.DividerHeight * (listView.Count - 1));
            Console.WriteLine("Total height + dividers = {0}", lparams.Height);
            listView.LayoutParameters = lparams;

        }
	}
}

