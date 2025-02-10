import unittest
from unittest.mock import patch
from your_module import main  # Replace 'your_module' with the actual module name

class TestNegativeScenarios(unittest.TestCase):

    @patch('your_module.load_dotenv')
    def test_load_dotenv_failure(self, mock_load_dotenv):
        mock_load_dotenv.side_effect = Exception("Failed to load .env file")
        with self.assertRaises(Exception) as context:
            main()
        self.assertIn("Failed to load .env file", str(context.exception))

    @patch('your_module.os.getenv')
    def test_getenv_failure(self, mock_getenv):
        mock_getenv.return_value = None
        with self.assertRaises(SystemExit) as context:
            main()
        self.assertEqual(context.exception.code, 1)

    @patch('your_module.Image.open')
    def test_image_open_failure(self, mock_image_open):
        mock_image_open.side_effect = IOError("Failed to open image file")
        with self.assertRaises(IOError) as context:
            main()
        self.assertIn("Failed to open image file", str(context.exception))

if __name__ == '__main__':
    unittest.main()