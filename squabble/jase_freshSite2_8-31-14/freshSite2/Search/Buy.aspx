<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Buy.aspx.cs" Inherits="freshSite2.Search.Buy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

     <h4>Squabble all over her face</h4>
        <hr />
    <div class="row">
        <div class="col-lg-8">
<div class="form-horizontal">
        

            <asp:Label runat="server" AssociatedControlID="lblIsbn1" CssClass="col-md-2 control-label">ISBN</asp:Label>
            <div class="col-md-10">
                <asp:Label runat="server" ID="lblIsbn1" CssClass="form-control"></asp:Label>
            </div>
                        <asp:Label runat="server" AssociatedControlID="lblTitle1" CssClass="col-md-2 control-label">Title</asp:Label>
                        <div class="col-md-10">
                            <asp:Label runat="server" ID="lblTitle1" CssClass="form-control"></asp:Label>
                        </div>


                         <asp:Label runat="server" AssociatedControlID="lblAuthor1" CssClass="col-md-2 control-label">Author</asp:Label>
                        <div class="col-md-10">
                            <asp:Label runat="server" ID="lblAuthor1" CssClass="form-control"></asp:Label>
                        </div>

                         <asp:Label runat="server" AssociatedControlID="lblEdition1" CssClass="col-md-2 control-label">Edition</asp:Label>
                        <div class="col-md-10">
                            <asp:Label runat="server" ID="lblEdition1" CssClass="form-control"></asp:Label>
                        </div>

                         <asp:Label runat="server" AssociatedControlID="lblPublisher1" CssClass="col-md-2 control-label">Publisher</asp:Label>
                        <div class="col-md-10">
                            <asp:Label runat="server" ID="lblPublisher1" CssClass="form-control"></asp:Label>
                        </div>
                    
   </div>
    </div>
        <div class="col-lg-4">
            <asp:Image ID="BookImage" runat="server"  />
        </div>
        
        </div>

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
