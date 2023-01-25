<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sell.aspx.cs" Inherits="freshSite2.Search.Sell" %>
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

    <br />
    <br />
     <div class="row">
        <div class="col-lg-10">
<div class="form-horizontal">

     <asp:Label runat="server" AssociatedControlID="tbPrice" CssClass="col-md-2 control-label">Your Price</asp:Label>
    <asp:RegularExpressionValidator id="RegularExpressionValidator1"
                   ControlToValidate="tbPrice"
                   ValidationExpression="\d+"
                   Display="Static"
                   EnableClientScript="true"
                   ErrorMessage="Numbers only, doucheface"
                   runat="server"/>                    
    <div class="col-md-10">
                            <asp:TextBox runat="server" ID="tbPrice" CssClass="form-control"></asp:TextBox>
                        </div>

    
                         <asp:Label runat="server" AssociatedControlID="dropQuality" CssClass="col-md-2 control-label">Quality</asp:Label>
                        <div class="col-md-10">
                            <asp:DropDownList ID="dropQuality" runat="server"></asp:DropDownList>
                        </div>

                        <asp:Label runat="server" AssociatedControlID="cbCD" CssClass="col-md-2 control-label">Contains CD ?</asp:Label>
                        <div class="col-md-10">
                            <asp:CheckBox ID="cbCD" runat="server" />
                        </div>
                        <asp:Label runat="server" AssociatedControlID="tbNotes" CssClass="col-md-2 control-label">Description and other notes</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox ID="tbNotes" runat="server" MaxLength="160" TextMode="MultiLine" Rows="3"></asp:TextBox>  
                        </div>
     <div class="col-md-10">
         <asp:Button ID="btnGo" runat="server" Text="Submit" OnClick="btnGo_Click" OnClientClick="return confirm('Are you sure?');" />
                        </div>

     </div>
    </div>
         </div>
</asp:Content>
