<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="bookDetail.aspx.cs" Inherits="freshSite2.bookDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="container">

        <div class ="row">

            <div class="col-lg-4">
                <asp:Label ID="Label1" runat="server" Text="ISBN"></asp:Label>
                 </div> 
            <div class="col-lg-4">
                <asp:Label ID="lblisbn" runat="server" Text="Label"></asp:Label>
                </div> 
            </div>
             <div class ="row">
            <div class="col-lg-4">
                <asp:Label ID="Label2" runat="server" Text="Title"></asp:Label>
                </div> 
                 <div class="col-lg-4">
                      <asp:Label ID="lblTitle" runat="server" Text="Label"></asp:Label>
                     </div> 
                 </div>
             <div class ="row">
                 <div class="col-lg-4">
                <asp:Label ID="Label3" runat="server" Text="Author"></asp:Label>
                    </div>
                    <div class="col-lg-4">
                        <asp:Label ID="lblAuthor" runat="server" Text="Label"></asp:Label>
                        </div>
                 </div>
                  <div class ="row">

                 <div class="col-lg-4">
                <asp:Label ID="Label4" runat="server" Text="Edition"></asp:Label>
                     </div>
                       <div class="col-lg-4">
                      <asp:Label ID="lblEdition" runat="server" Text="Label"></asp:Label>
                           </div>
                      </div>
                       <div class ="row">

                 <div class="col-lg-4">
                <asp:Label ID="Label5" runat="server" Text="Publisher"></asp:Label>
                     </div>
                            <div class="col-lg-4">
                                <asp:Label ID="lblPublisher" runat="server" Text="Label"></asp:Label>
            
                            </div>
                       </div>
        <</div>

    <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" >
                    <HeaderTemplate>
                        <table class="table table-striped table-bordered">
                            <tr>
                                <td><b>ISBN :</b></td>
                                <td><b>Title :</b></td>
                                <td><b>Price</b></td>
                                <td><b>Quality</b></td> 
                                <td><b>Squabble !</b></td>                              
                            </tr>
                        
                    </HeaderTemplate>
                    
                    <ItemTemplate>
                       
                        <a href="#">
                        <tr>
                           
                            <td>
                                <%# Int64.Parse(DataBinder.Eval(Container.DataItem, "ISBN").ToString()) < 50000
                                ? "N/A" : DataBinder.Eval(Container.DataItem, "ISBN")%> 
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Title").ToString()%> 
                            </td>
                          
                            <td>
                                <%# String.IsNullOrWhiteSpace(DataBinder.Eval(Container.DataItem, "InitialPrice").ToString())
                                ? "No Price" : DataBinder.Eval(Container.DataItem, "InitialPrice") %> 
                            </td>
                            <td>
                                <%# String.IsNullOrWhiteSpace(DataBinder.Eval(Container.DataItem, "Quality").ToString())
                                ? "N/A" : DataBinder.Eval(Container.DataItem, "Quality") %> 
                            </td>
                            <td>
                                <asp:Button Class="btn btn-success" CommandName='<%#DataBinder.Eval(Container.DataItem, "UserBookID")%>' ID="btnSquabble" runat="server" Text="Start Squabble!" />
                            </td>             
                                                    
                        </tr>
                            </a>
                    </ItemTemplate>
                    <FooterTemplate>
       
                        </table> 
                    </FooterTemplate>
                </asp:Repeater>
</asp:Content>
