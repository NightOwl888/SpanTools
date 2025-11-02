BeforeAll {
    . $PSCommandPath.Replace('.Tests.ps1','.ps1')
}

Describe "Format-Test-Results" {
    $testCases = @(
        @{ SuiteName="Alpha"; Passed=5; Failed=0; Ignored=16; Crashed=$false; Expected="✅ Passed" },
        @{ SuiteName="Beta";  Passed=1; Failed=2; Ignored=0; Crashed=$false; Expected="❌ Failed" },
        @{ SuiteName="Gamma"; Passed=0; Failed=0; Ignored=1; Crashed=$true;  Expected="⚠️ Crashed" }
    )

    It "produces expected status lines" -ForEach $testCases {
        $obj = [PSCustomObject]@{
            SuiteName    = $_.SuiteName
            PassedCount  = $_.Passed
            FailedCount  = $_.Failed
            IgnoredCount = $_.Ignored
            Crashed      = $_.Crashed
        }

        $output = Format-Test-Results $obj
        Write-Host $output -ForegroundColor Green
        $output | Should -Match $_.Expected
        $output | Should -Match "\<strong\>$($_.SuiteName)\</strong\>"
        $output | Should -Match "Passed=$($_.PassedCount)"
        $output | Should -Match "Failed=$($_.FailedCount)"
        $output | Should -Match "Ignored=$($_.IgnoredCount)"
    }

    Context "respects custom status text/icons" {
        It "respects Crashed" {
            $obj = [PSCustomObject]@{
                SuiteName="Delta"; PassedCount=0; FailedCount=0; IgnoredCount=0; Crashed=$true
            }

            $output = Format-Test-Results $obj `
                -IconCrashed 'XX' -TextCrashed 'Boom'
            $output | Should -Match "XX Boom"
        }
        
        It "respects Passed" {
            $obj = [PSCustomObject]@{
                SuiteName="Delta"; PassedCount=30; FailedCount=0; IgnoredCount=0; Crashed=$false
            }

            $output = Format-Test-Results $obj `
                -IconPassed 'YY' -TextPassed 'MePassed'
            $output | Should -Match "YY MePassed"
        }
        
        It "respects Failed" {
            $obj = [PSCustomObject]@{
                SuiteName="Delta"; PassedCount=30; FailedCount=2; IgnoredCount=0; Crashed=$false
            }

            $output = Format-Test-Results $obj `
                -IconFailed 'ZZ' -TextFailed 'MeFailed'
            $output | Should -Match "ZZ MeFailed"
        }
    }

    Context "matrix Options formatting" {

        It "includes options block when Options present" {
            $options = @{ MatrixId = 'MX_1234ABCD'; Foo='Bar'; Baz='Qux' }
            $obj = [PSCustomObject]@{
                SuiteName    = 'MatrixRun'
                PassedCount  = 2
                FailedCount  = 0
                IgnoredCount = 0
                Crashed      = $false
                Options      = $options
            }

            $output = Format-Test-Results $obj
            # details block + matrix summary
            $output | Should -Match '<details>'
            $output | Should -Match "\<strong\>MatrixRun\</strong\>"
        }

        It "renders option rows and escapes pipes" {
            $options = @{ MatrixId='MX_ABC'; Value1='one'; ValueWithPipe='a|b' }
            $obj = [PSCustomObject]@{
                SuiteName='MatrixRun2'
                PassedCount=0
                FailedCount=1
                IgnoredCount=0
                Crashed=$false
                Options=$options
            }

            $output = Format-Test-Results $obj

            # Check that option keys and values appear in table rows
            $output | Should -Match '\<tr\>\<td\>\s*Value1\s*\</td\>\<td\>\s*one\s*\</td\>\</tr\>'
            # the pipe character in the value should be escaped as '&#124;'
            $output | Should -Match '\<tr\>\<td\>\s*ValueWithPipe\s*\</td\>\<td\>\s*a&#124;b\s*\</td\>\</tr\>'
        }
    }
}
