from bs4 import BeautifulSoup
from typing import Tuple
import json


tests_report_html_file_name = [
    'mutation-report-integration-tests.html',
    'mutation-report-unit-tests.html'
]


def build_single_report(reports: dict) -> str:
    data = {}

    for report in reports:
        for key, value in json.loads(report).items():
            if key not in data:
                data[key] = value
            else:
                index = 0
                for mutantData in data[key]['mutants']:
                    for mutantValue in value['mutants']:
                        if mutantData['id'] == mutantValue['id'] and mutantData['status'] == 'Ignored' and mutantValue['status'] != 'Ignored':
                            data[key]['mutants'][index] = mutantValue
                    index = index + 1

    return json.dumps(data)


def build_template() -> Tuple[str, str]:
    header = ''
    footer = ''

    with open(tests_report_html_file_name[0], 'r', encoding = 'utf8') as f:
        data = str(BeautifulSoup(f.read(), 'html.parser'))
        data = data.split('"files":')
        header = data[0] + '"files":'
        footer = data[1].split('function updateTheme() {')[0].strip()[-4:] + 'function updateTheme() {' + data[1].split('function updateTheme() {')[1]
        f.close()

    return header, footer


def get_report_from(report_html_file_name: str) -> str:
    data = ''

    with open(report_html_file_name, 'r', encoding = 'utf8') as f:
        data = str(BeautifulSoup(f.read(), 'html.parser'))
        data = data.split('"files":')[1]
        data = data.split('function updateTheme() {')[0].split('}};')[0] + '}'
        f.close()

    return data


def execute():
    header, footer = build_template()

    reports = set()
    for test_report_html_file_name in tests_report_html_file_name:
        reports.add(get_report_from(test_report_html_file_name))

    report = build_single_report(reports)

    html_file = open('mutation-report.html', 'w', encoding = 'utf8')
    html_file.write(header + report.strip()[:-2] + footer)
    html_file.close()


execute()
