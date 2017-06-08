using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using OnlineFridge;
using test.DataAccess.Model;

namespace test
{
    class ListViewAdapterOnline: BaseAdapter<ProductOnline>
    {
        public List<ProductOnline> productsList;
        private Context context;

        public ListViewAdapterOnline(Context _context, List<ProductOnline> _productsList)
        {
            productsList = _productsList;
            context = _context;
        }

        public override int Count
        {
            get { return productsList.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override ProductOnline this[int position]
        {
            get { return productsList[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.listview_row, null, false);
            }

            TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
            txtName.Text = productsList[position].name;

            TextView txtCount = row.FindViewById<TextView>(Resource.Id.txtCount);
            txtCount.Text = productsList[position].count + " " + productsList[position].unit;

            TextView txtExpDate = row.FindViewById<TextView>(Resource.Id.txtExpDate);
            txtExpDate.Text = productsList[position].expDate;

            return row;
        }
    }

}