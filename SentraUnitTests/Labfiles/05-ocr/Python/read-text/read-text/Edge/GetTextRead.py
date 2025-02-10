import unittest
from unittest.mock import patch
from io import StringIO
from your_module import GetTextRead  # Replace 'your_module' with the actual module name

class TestGetTextRead(unittest.TestCase):

    @patch('builtins.print')
    def test_empty_image(self, mock_print):
        with open('empty.png', 'wb') as f:
            f.write(b'\x89PNG\r\n\x1a\n\x00\x00\x00\rIHDR\x00\x00\x00\x01\x00\x00\x00\x01\x08\x02\x00\x00\x00\x90wS\xde\x00\x00\x00\x0cIDATx\xdac\xf8\xff\xff?\x00\x05\xfe\x00\x00\x00\x00IEND\xaeB`\x82')

        GetTextRead('empty.png')
        mock_print.assert_called_once_with('\n')

    @patch('builtins.print')
    def test_large_image(self, mock_print):
        with open('large.png', 'wb') as f:
            f.write(b'\x89PNG\r\n\x1a\n\x00\x00\x00\rIHDR\x00\x00\x00\x80\x00\x00\x00\x80\x08\x02\x00\x00\x00\x90wS\xde\x00\x00\x00\x0cIDATx\xdac\xf8\xff\xff?\x00\x05\xfe\x00\x00\x00\x00IEND\xaeB`\x82')

        GetTextRead('large.png')
        mock_print.assert_called_once_with('\n')

    @patch('builtins.print')
    def test_single_pixel_image(self, mock_print):
        with open('single_pixel.png', 'wb') as f:
            f.write(b'\x89PNG\r\n\x1a\n\x00\x00\x00\rIHDR\x00\x00\x00\x01\x00\x00\x00\x01\x08\x02\x00\x00\x00\x90wS\xde\x00\x00\x00\x0cIDATx\xdac\xf8\xff\xff?\x00\x05\xfe\x00\x00\x00\x00IEND\xaeB`\x82')

        GetTextRead('single_pixel.png')
        mock_print.assert_called_once_with('\n')

if __name__ == '__main__':
    unittest.main()