import unittest
from unittest.mock import patch, MagicMock
from your_module import main  # Replace 'your_module' with the actual module name

class TestMain(unittest.TestCase):

    @patch('your_module.load_dotenv')
    def test_main_with_invalid_env(self, mock_load_dotenv):
        mock_load_dotenv.return_value = None
        with self.assertRaises(SystemExit) as context:
            main()
        self.assertEqual(context.exception.code, 1)

    @patch('your_module.Image.open')
    def test_main_with_invalid_image(self, mock_open):
        mock_open.side_effect = IOError("Invalid image")
        with self.assertRaises(IOError) as context:
            main()
        self.assertEqual(str(context.exception), "Invalid image")

    @patch('your_module.requests.get')
    def test_main_with_invalid_request(self, mock_get):
        mock_get.side_effect = requests.exceptions.RequestException("Failed request")
        with self.assertRaises(requests.exceptions.RequestException) as context:
            main()
        self.assertEqual(str(context.exception), "Failed request")

if __name__ == '__main__':
    unittest.main()