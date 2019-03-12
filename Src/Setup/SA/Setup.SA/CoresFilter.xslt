<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0"
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:wix="http://schemas.microsoft.com/wix/2006/wi"
				xmlns="http://schemas.microsoft.com/wix/2006/wi"
				exclude-result-prefixes="wix">

	<xsl:output method="xml" indent="yes" />

	<xsl:strip-space elements="*" />

	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="wix:Component">
		<Component Id="{@Id}" Guid="{@Guid}">
			<File Id="{wix:File/@Id}" Source="{concat('..\..\..\..\Refs\ScanCores', substring(wix:File/@Source, 21))}" />
		</Component>
	</xsl:template>
</xsl:stylesheet>