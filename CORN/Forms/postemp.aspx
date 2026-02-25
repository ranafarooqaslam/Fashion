<%@ Page Language="C#" AutoEventWireup="true" CodeFile="postemp.aspx.cs" Inherits="Forms_postemp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Untitled Document</title>
<link href="../css/POSstyle.css" rel="stylesheet" type="text/css" />
</head>

<body>
<div class="header">
	<div class="main">
   	  
    	<div class="header-t">
        	<div class="logo"></div>
        	<div class="menu-top">
            	<ul>
                	<li><a href="#"><img src="../images/icon-user.png" /><span>Customers<br /><b>F10</b></span></a></li>
                    <li><a href="#"><img src="../images/icon-hold.png" /><span>Hold<br /><b>Ctrl + H</b></span></a></li>
                    <li><a href="#"><img src="../images/icon-unhold.png" /><span>Unhold<br /><b>Ctrl + U</b></span></a></li>
                    <li><a href="#"><img src="../images/icon-price.png" /><span>Price Lookup<br /><b>F5</b></span></a></li>
                    <li><a href="#"><img src="../images/icon-search.png" /><span>Search<br /><b>F7</b></span></a></li>
                    <li><a href="#"><span style="margin-top:14px;">Receipts on hold</span><input type="text" value="00" /></a></li>
                </ul>
                <div class="shadow-t-menu"></div>
              
        	</div>
            
      </div>
        <div class="header-form">
                	<span><label>Customer</label><input name="" type="text" /><input name="" type="text" /></span>
                <span><label>Payment Type</label><input name="" type="radio" value="" /> <label>Cash</label><input name="" type="radio" value="" /> <label>Credit Card</label> <input name="" type="text" /><input name="" type="text" style="width:247px;" />
                  </span></div>
        
    </div>
</div>
<div class="menu2">
  <div class="main">
   	<ul>
        	<li class="sku-c">SKU Code</li>
            <li class="sperator"></li>
            <li class="sku-name">SKU Name</li>
            <li class="sperator"></li>
            <li class="color">Color</li>
            <li class="sperator"></li>
            <li class="size">Size</li>
            <li class="sperator"></li>
            <li class="qty">Quantity</li>
            <li class="sperator"></li>
            <li class="u-prize">Unit Price</li>
            <li class="sperator"></li>
            <li class="">Location:</li>
            
      </ul>
      <span><input type="text" value="Karim Block" /></span>
    </div>
</div>
<div class="main">
	<div class="r-pannel">
    	<span ><input name="" type="text" class="sku-c-input" /></span>
    	<span><input name="" type="text" class="sku-name-input" /></span>
    	<span><input name="" type="text" class="color-input" /></span>
    	<span><input name="" type="text" class="size-input" /></span>
    	<span><input name="" type="text" class="qty-input" /></span>
  		<span><input name="" type="text" class="u-prize-input" /></span>
    	<span class="btn-add"><a href="#"><img src="../images/btn-add.png"  /></a></span>
        <div class="clr"></div>
        <div class="grid"></div>
    </div>
    <div class="l-pannel">
    	<ul>
        	<li>
            	<span></span>
             	<img src="../images/cloth.png" />
             	<span></span>
             </li>
           	<li>
             	<label>Gross Sale</label>
                <input name="" type="text" />
             </li>
             <li>
             	<span></span>
             	<label>Discount</label>
                <input name="" type="text" />
             </li>
              <li>
             	<span></span>
             	<label>Sales Tax</label>
                <input name="" type="text" />
             </li>
             <li>
             	<span></span>
             	<label>Net Amount</label>
                <input name="" type="text" />
             </li>
             <li>
             	<span></span>
             	<label>Cash Received</label>
                <input name="" type="text" />
             </li>
             <li>
             	<span></span>
             	<label>Balance</label>
                <input name="" type="text" />
             </li>
             
        </ul>
    </div>
</div>
<div class="footer">
	<div class="main">
    	<div class="address">
       	  <h2>Arshad Sons limited</h2>
          <p><b>Address: </b><span>Asif Block, Allama Iqbal Town, Lahore</span></p>
            <p><b>Phone: </b></p>
            <p><b>Fax: </b></p>
            <p><b>Email: </b></p>
        </div>
      <div class="shadow-footer"></div>
        <div class="user-login">
   	    	<img src="../images/user-login.png"  /> 
            <span>
            	<h2>User Login</h2>
                <h1>Amjad</h1>
                <p><b>Group:</b> Cashiers</p>
                <p><b>Loged in at:</b> 05/13,  12:32 p.m</p>
            </span>
        </div>
        <div class="shadow-footer"></div>
      <div class="free-sku">
        	<h2>Free Sku</h2>
        <div class="grid2"></div>
          <p> 
          	<a href="#"><img src="../images/btn-calculate.png" /></a> 
          	<a href="#"><img src="../images/btn-save.png"  /></a>
          	<a href="#"><img src="../images/btn-exit.png" /></a>
          </p>
        </div> 
    </div>
</div>
</body>
</html>
