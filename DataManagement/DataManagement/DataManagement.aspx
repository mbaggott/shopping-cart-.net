<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataManagement.aspx.cs" Inherits="DataManagement.DataManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title></title>
   <style type="text/css">
      .auto-style1 {
         width: 156px;
      }
   </style>
</head>
<body>
   <form id="form1" runat="server">
      <div>
         <%--  Insert a dropdown list populated with a data source from the database --%>
         <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource2" DataTextField="Title" DataValueField="CategoryID" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"></asp:DropDownList>
         <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
         &nbsp;&nbsp;&nbsp;
         <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" />
         <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [Category]"></asp:SqlDataSource>
         
         <%-- Add a grid view which will dynamically respond to events --%>
         <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="ProductID" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit" OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" EmptyDataText="No records have been entered" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">

            <%-- Draw the main display grid with widths, titles and textboxes, displaying the buttons --%>
            <Columns>
               <asp:TemplateField HeaderText="Category ID" ItemStyle-Width="150">
                  <ItemTemplate>
                     <asp:Label ID="lblCategoryId" runat="server" Text='<%# Eval("CategoryID") %>'></asp:Label>
                  </ItemTemplate>
                  <EditItemTemplate>
                     <asp:TextBox ID="txtCategoryId" runat="server" Text='<%# Eval("CategoryID") %>'></asp:TextBox>
                  </EditItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="Title" ItemStyle-Width="150">
                  <ItemTemplate>
                     <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                  </ItemTemplate>
                  <EditItemTemplate>
                     <asp:TextBox ID="txtTitle" runat="server" Text='<%# Eval("Title") %>'></asp:TextBox>
                  </EditItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="Short Description" ItemStyle-Width="150">
                  <ItemTemplate>
                     <asp:Label ID="lblShortDescription" runat="server" Text='<%# Eval("ShortDescription") %>'></asp:Label>
                  </ItemTemplate>
                  <EditItemTemplate>
                     <asp:TextBox ID="txtShortDescription" runat="server" Text='<%# Eval("ShortDescription") %>'></asp:TextBox>
                  </EditItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="Long Description" ItemStyle-Width="150">
                  <ItemTemplate>
                     <asp:Label ID="lblLongDescription" runat="server" Text='<%# Eval("LongDescription") %>'></asp:Label>
                  </ItemTemplate>
                  <EditItemTemplate>
                     <asp:TextBox ID="txtLongDescription" runat="server" Text='<%# Eval("LongDescription") %>'></asp:TextBox>
                  </EditItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="Image URL" ItemStyle-Width="260">
                  <ItemTemplate>
                     <asp:Label ID="lblImageUrl" runat="server" Text='<%# Eval("ImageUrl") %>'></asp:Label>
                  </ItemTemplate>
                  <EditItemTemplate>
                     <asp:TextBox ID="txtImageUrl" runat="server" Text='<%# Eval("ImageUrl") %>'></asp:TextBox>
                  </EditItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="Price" ItemStyle-Width="150">
                  <ItemTemplate>
                     <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
                  </ItemTemplate>
                  <EditItemTemplate>
                     <asp:TextBox ID="txtPrice" runat="server" Text='<%# Eval("Price") %>'></asp:TextBox>
                  </EditItemTemplate>
               </asp:TemplateField>
               <asp:CommandField ButtonType="Link" ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="150" />
            </Columns>
            <%-- Some basic table formatting for display --%>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
         </asp:GridView>

         <%-- Draw a table below the main display grid with all the relevant input fields and 'Add' button for
              inserting into the database --%>
         <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
            <tr>
               <td class="auto-style1">CategoryId<br />
                  <asp:TextBox ID="txtCategoryId" runat="server" Width="150" OnTextChanged="txtCategoryId_TextChanged" />
               </td>
               <td class="auto-style1">Title<br />
                  <asp:TextBox ID="txtTitle" runat="server" Width="150" />
               </td>
               <td class="auto-style1">Short Description<br />
                  <asp:TextBox ID="txtShortDescription" runat="server" Width="150" />
               </td>
               <td class="auto-style1">Long Description<br />
                  <asp:TextBox ID="txtLongDescription" runat="server" Width="150" />
               </td>
               <td class="auto-style1">Image URL<br />
                  <asp:TextBox ID="txtImageUrl" runat="server" Width="260" />
               </td>
               <td class="auto-style1">Price<br />
                  <asp:TextBox ID="txtPrice" runat="server" Width="150" />
               </td>
               <td class="auto-style1"><br />
                  <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="Insert" />
               </td>
            </tr>
         </table>
      </div>
   </form>
</body>
</html>
