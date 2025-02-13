import unittest
from unittest.mock import patch
from your_module import main

class TestMain(unittest.TestCase):

    @patch('your_module.load_dotenv')
    def test_main_invalid_dotenv(self, mock_load_dotenv):
        mock_load_dotenv.side_effect = Exception("Invalid .env file")
        with self.assertRaises(Exception) as context:
            main()
        self.assertIn("Invalid .env file", str(context.exception))

    @patch('your_module.os.getenv')
    def test_main_missing_env_variable(self, mock_getenv):
        mock_getenv.return_value = None
        with self.assertRaises(KeyError) as context:
            main()
        self.assertIn("Environment variable not set", str(context.exception))

    @patch('your_module.Image.open')
    def test_main_image_open_failure(self, mock_open):
        mock_open.side_effect = IOError("Failed to open image")
        with self.assertRaises(IOError) as context:
            main()
        self.assertIn("Failed to open image", str(context.exception))

if __name__ == '__main__':
    unittest.main()