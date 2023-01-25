<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="freshSite2.MyAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 
    <div class="page-header">
        
                <h3><span class="glyphicon glyphicon-book"></span>Welcome <%: Context.User.Identity.GetUserName()  %></h3>
        <div class="container">
                <div class="row" style="display: table; width: 70%; grid-column-align: center">
                    <div class="col-md-4" style="height: 100px; background-color: #ff961d; border-style: solid; border-width: 1px; display: table-cell; text-align: center; vertical-align: middle;">My Active Squabbles</div>
                    <div class="col-md-4" style="height: 100px; background-color: #ff961d; border-style: solid; border-width: 1px; display: table-cell; text-align: center; vertical-align: middle;">My Past Squabbles</div>
                    <div class="col-md-4" style="height: 100px; background-color: #ff961d; border-style: solid; border-width: 1px; display: table-cell; text-align: center; vertical-align: middle;">My Account Information</div>
                </div>
            </div>
        </div>

    <ul class="nav nav-tabs" id="rowTab">
        <li class="active"><a href="#buyingPanel" data-toggle="tab">Buying</a></li>
        <li><a href="#sellingPanel" data-toggle="tab">Selling</a></li>
    </ul>
    <div class="tab-content">
        <div class="tab-pane active" id="buyingPanel">
            <asp:Panel ID="pnlBuying" runat="server"></asp:Panel>
        </div>
        <div class="tab-pane" id="sellingPanel">
            <asp:Panel ID="pnlSelling" runat="server"></asp:Panel>
        </div>
    </div>
</asp:Content>
