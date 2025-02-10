import unittest
from unittest.mock import patch, mock_open
from io import StringIO
import sys

class TestMain(unittest.TestCase):

    @patch('builtins.open')
    def test_load_dotenv_with_empty_file(self, mock_open):
        mock_open.return_value = mock_open(read_data='').return_value
        with self.assertRaises(SystemExit) as context:
            main()
        self.assertEqual(context.exception.code, 0)

    @patch('builtins.open')
    def test_load_dotenv_with_valid_content(self, mock_open):
        mock_open.return_value = mock_open(read_data='API_KEY=test_key\n').return_value
        with self.assertRaises(SystemExit) as context:
            main()
        self.assertEqual(context.exception.code, 0)

    @patch('os.environ.get')
    @patch('time.sleep')
    @patch('PIL.Image.new')
    @patch('PIL.ImageDraw.Draw')
    @patch('matplotlib.pyplot.show')
    def test_main_functionality(self, mock_show, mock_draw, mock_image, mock_sleep, mock_get):
        mock_get.side_effect = ['test_key', 'image_path']
        mock_image.return_value = mock_image
        mock_draw.return_value = mock_draw
        with self.assertRaises(SystemExit) as context:
            main()
        self.assertEqual(context.exception.code, 0)
        mock_sleep.assert_called_once_with(10)
        mock_image.assert_called_once_with('RGB', (100, 100))
        mock_draw.assert_called_once()
        mock_show.assert_called_once()

if __name__ == '__main__':
    unittest.main()