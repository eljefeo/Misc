<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchBooks.aspx.cs" Inherits="freshSite2.Search.SearchBooks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">





         <div>
            <h3>Search</h3>
            <p>

                <aspPlaceHolder Id="ph1" runat="server"></aspPlaceHolder>

                <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" >
                    <HeaderTemplate>
                        <table class="table table-striped table-bordered">
                            <tr>
                                <td><b>ISBN</b></td>
                                <td><b>Title</b></td>
                                <td><b>Author</b></td>
                                <td><b>Edition</b></td>
                                <td><b>Publisher</b></td>
                                <td><b>Availability</b></td>
                                <td><b>Sell</b></td>                               
                            </tr>
                        
                    </HeaderTemplate>
                    
                    <ItemTemplate>
                       
                        <a href="#">
                        <tr>
                           
                            <td>
                                <%# isbnReturn(DataBinder.Eval(Container.DataItem, "ISBN").ToString()) %> 
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Title") %> 
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Author") %> 
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Edition") %> 
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Publisher") %> 
                            </td>
                            <td>
                                
                                    <b>
                                <%# String.IsNullOrWhiteSpace(DataBinder.Eval(Container.DataItem, "Quantity").ToString())
                                ? 0 : DataBinder.Eval(Container.DataItem, "Quantity") %> 
                                </b>
                                &nbsp;&nbsp;
                             <asp:Button Class="btn btn-success" Enabled=<%#setVisible(DataBinder.Eval(Container.DataItem, "Quantity").ToString())%> UseSubmitBehavior="false" ID="Button1" CommandName='<%#getIsbn("0")%>' runat="server" Text="Buy" />

                            </td>
                       
                             <td>
                                <asp:Button Class="btn btn-success"  CommandName='<%#getIsbn()%>' UseSubmitBehavior="false" ID="Button2" runat="server" Text="Sell" />
                            </td>             
                                                    
                        </tr>
                            </a>
                    </ItemTemplate>
                    <FooterTemplate>
       
                        </table> 
                    </FooterTemplate>
                </asp:Repeater>
            </p>
        </div>


 
</asp:Content>
