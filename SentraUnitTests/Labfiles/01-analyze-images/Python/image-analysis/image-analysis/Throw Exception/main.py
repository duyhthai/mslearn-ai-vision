import unittest
from unittest.mock import patch, MagicMock
from your_module import main  # Replace 'your_module' with the actual module name

class TestMain(unittest.TestCase):

    @patch('your_module.load_dotenv')
    def test_load_dotenv_exception(self, mock_load_dotenv):
        mock_load_dotenv.side_effect = Exception("Mocked exception")
        with self.assertRaises(Exception) as context:
            main()
        self.assertIn("Mocked exception", str(context.exception))

    @patch('your_module.os.getenv')
    def test_os_getenv_exception(self, mock_getenv):
        mock_getenv.return_value = None
        with self.assertRaises(SystemExit) as context:
            main()
        self.assertEqual(context.exception.code, 1)

    @patch('your_module.Image.open')
    def test_image_open_exception(self, mock_open):
        mock_open.side_effect = IOError("Mocked IOError")
        with self.assertRaises(IOError) as context:
            main()
        self.assertIn("Mocked IOError", str(context.exception))

if __name__ == '__main__':
    unittest.main()