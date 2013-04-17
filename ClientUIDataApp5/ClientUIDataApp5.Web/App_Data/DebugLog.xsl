<?xml version="1.0" encoding="iso-8859-1"?>

<xsl:stylesheet 
	version="2.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	>
<xsl:template match="/">
  <html>
    <head>
      <title>DevForce 2012 Debug Log</title>
      <style>
      table {
        background-color: #222299;
      }
      th {
        background-color: #cccccc;
        text-align: center;
        font-weight: bold;
      }
      td {	
        background-color: #ffffff;
        font-size: small;
        padding-left: 2px;padding-right: 2px;
          vertical-align:text-top;
      }
      .row2 td {
        background-color: #cccccc;
      }
      </style>
    </head>
  <body>
    <table class="logtable">
    <tr>
		<th>Date</th>
		<th>Time</th>
		<th>UserName</th>
		<th>Namespace.Class:Method</th>
		<th>Message</th>
    </tr>
    <xsl:apply-templates select="log/entry"/>
    </table>
  </body>
  </html>
</xsl:template>

<xsl:template match="entry">
	<xsl:if test="position()  mod 2 = 0">
		<tr class="row2">
		<td style="white-space:nowrap"><xsl:value-of select="substring-before(@timestamp,'T')"/></td>
    <td><xsl:value-of select="substring-after(@timestamp,'T')"/></td>
		<td><xsl:value-of select="@username"/></td>
		<td><xsl:value-of select="@source"/></td>
    <td><xsl:value-of select="."/></td>
		</tr>
	</xsl:if>
	<xsl:if test="position()  mod 2 = 1">
		<tr class="row1">
		<td style="white-space:nowrap"><xsl:value-of select="substring-before(@timestamp,'T')"/></td>
    <td><xsl:value-of select="substring-after(@timestamp,'T')"/></td>
		<td><xsl:value-of select="@username"/></td>
		<td><xsl:value-of select="@source"/></td>
    <td><xsl:value-of select="."/></td>
		</tr>
	</xsl:if>
	
</xsl:template>
</xsl:stylesheet>


