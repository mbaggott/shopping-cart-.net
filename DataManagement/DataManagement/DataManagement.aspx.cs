using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace DataManagement
{
   public partial class DataManagement : System.Web.UI.Page
   {
      /* Declare commonly used connection string*/
      string ConnectionString = ConfigurationManager.ConnectionStrings
      ["ConnectionString"].ConnectionString;

      /*********************************************************************/
      /************************* Initial page load *************************/
      /*********************************************************************/
      /* Load the page with all store items displayed on first launch*/
      protected void Page_Load(object sender, EventArgs e)
      {
         /* If it's the first load, bind the grid with all the items*/
         if (!this.IsPostBack)
         {
            this.BindGrid("SELECT");
         }
      }

      /*********************************************************************/
      /**************************** Bind Grid ******************************/
      /*********************************************************************/
      /* Form the grid with the passed-in parameter and defaults*/
      private void BindGrid(string argument)
      {
         refreshGrid(null, null, 0, argument);
      }

      /*********************************************************************/
      /************************** On editing row ***************************/
      /*********************************************************************/
      /* Re-form the grid based on the newly updated values*/
      protected void OnRowEditing(object sender, GridViewEditEventArgs e)
      {
         /* Get index of item being edited, get it's value and re-form
            the grid, displaying the product category view*/
         GridView1.EditIndex = e.NewEditIndex;
         int categoryId = Convert.ToInt32(DropDownList1.SelectedValue);
         refreshGrid(sender, e, categoryId, "SELECT-CATEGORY");
      }

      /*********************************************************************/
      /************************ On cancelling edit *************************/
      /*********************************************************************/
      /* Re-form the grid, cancelling any changes*/
      protected void OnRowCancelingEdit(object sender, EventArgs e)
      {
         /* Cancel the editing of the index and re-form the grid, displaying
            the product category*/
         GridView1.EditIndex = -1;
         int categoryId = Convert.ToInt32(DropDownList1.SelectedValue);
         refreshGrid(sender, e, categoryId, "SELECT-CATEGORY");
      }

      /*********************************************************************/
      /************************ Inserting into grid ************************/
      /*********************************************************************/
      /* Insert entered values into the database and grid, and update view*/
      protected void Insert(object sender, EventArgs e)
      {
         /* Gather entered product details for inserting into DB*/
         int id = Convert.ToInt32(DropDownList1.SelectedValue);
         string title = txtTitle.Text;
         string shortDesc = txtShortDescription.Text;
         string longDesc = txtLongDescription.Text;
         string imageUrl = txtImageUrl.Text;
         decimal price = Convert.ToDecimal(txtPrice.Text);

         /* Create connections and send all the parameters into the SPROC
            for inserting into the database*/
         using (SqlConnection con = new SqlConnection(ConnectionString))
         {
            using (SqlCommand cmd = new SqlCommand("ProductsSPROC"))
            {
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@Action", "INSERT");
               cmd.Parameters.AddWithValue("@CategoryId", id);
               cmd.Parameters.AddWithValue("@Title", title);
               cmd.Parameters.AddWithValue("@ShortDescription", shortDesc);
               cmd.Parameters.AddWithValue("@LongDescription", longDesc);
               cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
               cmd.Parameters.AddWithValue("@Price", price);
               cmd.Connection = con;
               con.Open();
               cmd.ExecuteNonQuery();
               con.Close();
            }
         }
         /* Refresh the grid view, displaying the product category only and
            clear all the input fields*/
         refreshGrid(sender, e, id, "SELECT-CATEGORY");

         txtCategoryId.Text = String.Empty;
         txtTitle.Text = String.Empty;
         txtShortDescription.Text = String.Empty;
         txtLongDescription.Text = String.Empty;
         txtImageUrl.Text = String.Empty;
         txtPrice.Text = String.Empty;
      }

      /*********************************************************************/
      /********************** Updating the table row ***********************/
      /*********************************************************************/
      /* Update all the product data and re-load into database*/
      protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
      {
         /* Display editable version of all the row data fields for editing*/
         GridViewRow row = GridView1.Rows[e.RowIndex];
         int productId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex]
            .Values[0]);
         int categoryId = Convert.ToInt32(DropDownList1.SelectedValue);
         string title = (row.FindControl("txtTitle") as TextBox).Text;
         string shortDescription = (row.FindControl("txtShortDescription") 
            as TextBox).Text;
         string longDescription = (row.FindControl("txtLongDescription") 
            as TextBox).Text;
         string imageUrl = (row.FindControl("txtImageUrl") as TextBox).Text;
         string price = (row.FindControl("txtPrice") as TextBox).Text;

         /* Create connections and pass edited information into the SPROC
            for inserting into the database*/
         using (SqlConnection con = new SqlConnection(ConnectionString))
         {
            using (SqlCommand cmd = new SqlCommand("ProductsSPROC"))
            {
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@Action", "UPDATE");
               cmd.Parameters.AddWithValue("@ProductId", productId);
               cmd.Parameters.AddWithValue("@CategoryId", categoryId);
               cmd.Parameters.AddWithValue("@Title", title);
               cmd.Parameters.AddWithValue("@ShortDescription", 
                  shortDescription);
               cmd.Parameters.AddWithValue("@LongDescription", 
                  longDescription);
               cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
               cmd.Parameters.AddWithValue("@Price", price);
               cmd.Connection = con;
               con.Open();
               cmd.ExecuteNonQuery();
               con.Close();
            }
         }
         /* Stop editing the index and re-form the view in the category
            view*/
         GridView1.EditIndex = -1;
         refreshGrid(sender, e, categoryId, "SELECT-CATEGORY");
      }

      /*********************************************************************/
      /*************************** Deleting a row **************************/
      /*********************************************************************/
      /* Delete the entire product row from the database*/
      protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
      {
         /* Get product id of the selected item to be deleted and determine
            which category it is*/
         int productId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex]
            .Values[0]);
         int categoryId = Convert.ToInt32(DropDownList1.SelectedValue);

         /* Create connections and call the SPROC to delete the row from DB*/
         using (SqlConnection con = new SqlConnection(ConnectionString))
         {
            using (SqlCommand cmd = new SqlCommand("ProductsSPROC"))
            {
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@Action", "DELETE");
               cmd.Parameters.AddWithValue("@ProductId", productId);
               cmd.Connection = con;
               con.Open();
               cmd.ExecuteNonQuery();
               con.Close();
            }
         }

         /* Re-form the grid, displaying the category view*/
         refreshGrid(sender, e, categoryId, "SELECT-CATEGORY");
      }

      /*********************************************************************/
      /************************** Display category *************************/
      /*********************************************************************/
      /* Display the category as selected in the dropdown box*/
      protected void btnSubmit_Click(object sender, EventArgs e)
      {
         /* Get the selected category id and send to the SPROC for displaying
            the relevant product category*/
         int categoryId = Convert.ToInt32(DropDownList1.SelectedValue);
         refreshGrid(sender, e, categoryId, "SELECT-CATEGORY");
      }

      /*********************************************************************/
      /************************ Deleting a category ************************/
      /*********************************************************************/
      /* Delete the category selected in the dropdown box*/
      protected void btnDelete_Click(object sender, EventArgs e)
      {
         /* Get the category id and send it into the SPROC to remove the
            whole category from the database and re-populate the dropdown*/
         int categoryId = Convert.ToInt32(DropDownList1.SelectedValue);

         refreshGrid(sender, e, categoryId, "DELETE-CATEGORY");
         DataBind();
      }

      /*********************************************************************/
      /************************* Re-form the grid **************************/
      /*********************************************************************/
      /* Handle the grid refresh for all the above methods*/
      protected void refreshGrid(object sender, EventArgs e, int id, 
         string sprocParameter)
      {
         /* Create connections and call the SPROC to refresh the database*/
         using (SqlConnection con = new SqlConnection(ConnectionString))
         {
            /* Create and declare command parameters and call relevant SPROC, 
               passing in parameter*/
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "ProductsSPROC";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", sprocParameter);
            cmd.Parameters.AddWithValue("@CategoryID", id);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            /* Re-form the grid and complete*/
            sda.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            con.Close();
         }
      }

      protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
      {

      }

      protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
      {

      }

      protected void txtCategoryId_TextChanged(object sender, EventArgs e)
      {

      }
   }
}