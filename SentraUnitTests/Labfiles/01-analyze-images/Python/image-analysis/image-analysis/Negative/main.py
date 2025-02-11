import unittest
from unittest.mock import patch, mock_open
from io import StringIO

class TestMain(unittest.TestCase):

    @patch('builtins.open')
    def test_load_dotenv_missing_file(self, mock_open):
        mock_open.side_effect = FileNotFoundError
        with self.assertRaises(FileNotFoundError):
            main()

    @patch.dict(os.environ, {'AZURE_KEY': 'invalid_key'})
    def test_azure_invalid_key(self):
        with self.assertRaises(Exception) as context:
            main()
        self.assertIn("Invalid Azure key", str(context.exception))

    @patch('requests.get')
    def test_requests_get_exception(self, mock_get):
        mock_get.side_effect = requests.exceptions.RequestException
        with self.assertRaises(requests.exceptions.RequestException):
            main()

if __name__ == '__main__':
    unittest.main()