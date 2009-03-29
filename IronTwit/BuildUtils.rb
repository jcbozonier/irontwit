require 'erb'

class NUnitRunner
	include FileTest

	def initialize(paths)
		@sourceDir = paths.fetch(:source, 'source')
		@resultsDir = paths.fetch(:results, 'results')
		@compilePlatform = paths.fetch(:platform, 'x86')
		@compileTarget = paths.fetch(:compilemode, 'debug')
		
		if ENV["teamcity.dotnet.nunitlauncher"] # check if we are running in TeamCity
			# We are not using the TeamCity nunit launcher. We use NUnit with the TeamCity NUnit Addin which needs tO be copied to our NUnit addins folder
			# http://blogs.jetbrains.com/teamcity/2008/07/28/unfolding-teamcity-addin-for-nunit-secrets/
			# The teamcity.dotnet.nunitaddin environment variable is not available until TeamCity 4.0, so we hardcode it for now
			@teamCityAddinPath = ENV["teamcity.dotnet.nunitaddin"] ? ENV["teamcity.dotnet.nunitaddin"] : 'c:/TeamCity/buildAgent/plugins/dotnetPlugin/bin/JetBrains.TeamCity.NUnitAddin-NUnit'
			cp @teamCityAddinPath + '-2.4.7.dll', 'tools/nunit/addins'
		end
	
		@nunitExe = File.join('ThirdParty', 'UnitTesting', "nunit-console#{(@compilePlatform.nil? ? '' : "-#{@compilePlatform}")}.exe").gsub('/','\\') + ' /nothread'
	end
	
	def executeTests(assemblies)
		createResultDir()
		
		assemblies.each do |assem|
			file = File.expand_path("#{@sourceDir}/#{assem}/bin/#{(@compilePlatform.nil? ? '' : "#{@compilePlatform}/")}#{@compileTarget}/#{assem}.dll")
			executeTestOnAssembly(file, assem)
		end
	end
	
	def executeTestInProject(projectName, assembly)
		createResultDir()
		
		file = File.expand_path("#{@sourceDir}/#{projectName}/bin/#{(@compilePlatform.nil? ? '' : "#{@compilePlatform}/")}#{@compileTarget}/#{assembly}.dll")
		executeTestOnAssembly(file, projectName)
	end
	
	def createResultDir
		Dir.mkdir @resultsDir unless exists?(@resultsDir)
	end
	
	def executeTestOnAssembly(file, assemblyName)
		sh "#{@nunitExe} \"#{file}\" /xml:" + File.join(@resultsDir, "#{assemblyName}-test.xml")
	end
end

class MSBuildRunner
	def self.compile(attributes)
		version = attributes.fetch(:clrversion, 'v3.5')
		compileTarget = attributes.fetch(:compilemode, 'debug')
	    solutionFile = attributes[:solutionfile]
		
		frameworkDir = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', version)
		msbuildFile = File.join(frameworkDir, 'msbuild.exe')
		
		sh "#{msbuildFile} #{solutionFile} /maxcpucount /v:m /property:BuildInParallel=false /property:Configuration=#{compileTarget} /t:Rebuild"
	end
end

class AsmInfoBuilder
	attr_reader :buildnumber

	def initialize(asmVersion, properties)
		@properties = properties;
		
		@buildnumber = asmVersion
		@properties['Version'] = @properties['InformationalVersion'] = buildnumber;
	end

	def write(baseFilename, language)
		if (language.casecmp("vb") == 0) then writeVB(baseFilename) end
		if (language.casecmp("cs") == 0) then writeCS(baseFilename) end
		raise "Specified Language for AsmInfo not 'CS' or 'VB'"
	end
	
	def writeCS(baseFilename)
		csTemplate = %q{
using System;
using System.Reflection;
using System.Runtime.InteropServices;

<% @properties.each {|k, v| %>
[assembly: Assembly<%=k%>Attribute("<%=v%>")]
<% } %>
		}.gsub(/^    /, '')
		  
		filename = baseFilename + ".cs"
		writeAsmInfo(filename, csTemplate)
	end
	
	def writeVB(baseFilename)	
		vbTemplate = %q{
Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

<% @properties.each {|k, v| %>
<Assembly: Assembly<%=k%>Attribute("<%=v%>")>
<% } %>
		}.gsub(/^    /, '')
		
		filename = baseFilename + ".vb"
		writeAsmInfo(filename, vbTemplate)
	end		
	
	def writeAsmInfo(filename, template)
		erb = ERB.new(template, 0, "%<>")
	  
		File.open(filename, 'w') do |file|
			file.puts erb.result(binding) 
		end
	end
end