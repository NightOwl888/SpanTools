<#
.SYNOPSIS
    Formats test results into a Markdown summary with icons and status text.

.DESCRIPTION
    The Format-Test-Results function takes one or more test result objects
    (typically PSCustomObjects with fields such as SuiteName, PassedCount,
    FailedCount, IgnoredCount, and Crashed) and produces a Markdown string
    summarizing the results. Each test suite is displayed with an icon,
    status text, and counts of passed, failed, and ignored tests.

    When a result object includes an 'Options' property (a hashtable produced
    from the test matrix .props), the function will append an expandable
    details block showing the options as a two-column Markdown table.
#>

function Format-Test-Results {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, ValueFromPipeline, Position=0)]
        [ValidateNotNullOrEmpty()]
        [PSCustomObject[]] $Results,

        # Icons/texts as parameters (with defaults)
        [string] $IconPassed  = '✅',
        [string] $TextPassed  = 'Passed',

        [string] $IconFailed  = '❌',
        [string] $TextFailed  = 'Failed',

        [string] $IconCrashed = '⚠️',
        [string] $TextCrashed = 'Crashed'
    )

    begin {
        $sb = [System.Text.StringBuilder]::new()
        [void]$sb.AppendLine("## Test Results`n")
    }

    process {
        # helper to make an option value safe for markdown tables:
        function Escape-For-MarkdownTable([string]$s) {
            if ($null -eq $s) { return '' }
            # escape pipe using html entity and replace CR/LF with <br/>
            $out = $s -replace '\|','&#124;'
            $out = $out -replace "`r`n","<br/>"
            $out = $out -replace "`n","<br/>"
            return $out
        }

        foreach ($r in $Results) {
            if ($r.Crashed) {
                $statusIcon = $IconCrashed
                $statusText = $TextCrashed
            }
            elseif ($r.FailedCount -gt 0) {
                $statusIcon = $IconFailed
                $statusText = $TextFailed
            }
            else {
                $statusIcon = $IconPassed
                $statusText = $TextPassed
            }

            # Basic one-line summary
            $summary = "$statusIcon $statusText - <strong>$($r.SuiteName)</strong> " +
                    "| Passed=$($r.PassedCount), Failed=$($r.FailedCount), Ignored=$($r.IgnoredCount)"

            if ($r.PSObject.Properties.Name -contains 'Options' -and $null -ne $r.Options) {
                # If Options exist, render an expandable block with a small two-column table

                [void]$sb.AppendLine("  <details>")
                [void]$sb.AppendLine("  <summary>$summary (click to expand)</summary>")
                [void]$sb.AppendLine("<br/>")  # <-- ensures spacing between summary and table

                # Indent visually and add some padding around table cells.
                [void]$sb.AppendLine('  <div style="margin-left: 16px;">')
                [void]$sb.AppendLine('  <table cellpadding="4" style="border-collapse: collapse;">')
                [void]$sb.AppendLine('    <thead><tr><th align="left">Option</th><th align="left">Value</th></tr></thead>')
                [void]$sb.AppendLine('    <tbody>')

                # gather keys in stable order depending on Options type
                $keys = @()
                try {
                    $keys = $r.Options.Keys | Sort-Object
                } catch {
                    # if Options isn't dictionary-like, fallback to PSObject properties
                    $keys = $r.Options.PSObject.Properties.Name | Sort-Object
                }

                foreach ($k in $keys) {
                    # Skip the MatrixID, since it is not an option
                    if ($k -eq 'MatrixId') { continue }
                    $v = $null
                    try { $v = $r.Options[$k] } catch { $v = $r.Options.$k }
                    $ke = Escape-For-MarkdownTable([string]$k)
                    $ve = Escape-For-MarkdownTable([string]$v)

                    [void]$sb.AppendLine("      <tr><td>$ke</td><td>$ve</td></tr>")
                }

                [void]$sb.AppendLine('    </tbody>')
                [void]$sb.AppendLine('  </table>')
                [void]$sb.AppendLine('  </div>')
                [void]$sb.AppendLine("<br/>")  # <-- adds space after table
                [void]$sb.AppendLine("  </details>")
                [void]$sb.AppendLine()
            } else {
                [void]$sb.Append("- ")
                [void]$sb.AppendLine($summary)
            }
        }
    }

    end {
        return $sb.ToString()
    }
}
