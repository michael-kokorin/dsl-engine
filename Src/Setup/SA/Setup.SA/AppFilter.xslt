<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0"
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:wix="http://schemas.microsoft.com/wix/2006/wi">

	<xsl:output method="xml" indent="yes" />

	<xsl:strip-space elements="*" />

	<xsl:template match="@*|*">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates select="*" />
		</xsl:copy>
	</xsl:template>

	<xsl:key name="dir-logs" match="wix:Directory[@Name = 'logs']" use="@Id" />
	<xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, '.pdb')]" use="@Id" />
	<xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, '.xml')]" use="@Id" />
	<xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, '.pssym')]" use="@Id" />
	<xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, '.dll.config')]" use="@Id" />
	<xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, '.vshost.exe')]" use="@Id" />
	<xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, 'app.publish')]" use="@Id" />
	<xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, '.application')]" use="@Id" />
	<xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, 'logs')]" use="@Id" />

	<xsl:template match="wix:Directory[@Name='logs']" />
	<xsl:template match="wix:Component[key('dir-logs', @Directory)]" />
	<xsl:template match="wix:Fragment[wix:DirectoryRef[key('dir-logs', @Id)]]" />

	<xsl:template match="wix:Component[key('service-search', @Id)]" />
	<xsl:template match="wix:ComponentRef[key('service-search', @Id)]" />
</xsl:stylesheet>