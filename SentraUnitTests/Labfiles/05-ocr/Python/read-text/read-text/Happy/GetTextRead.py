import unittest
from unittest.mock import patch
import io
from PIL import Image

class TestGetTextRead(unittest.TestCase):

    @patch('builtins.print')
    def test_get_text_read(self, mock_print):
        # Arrange
        image = Image.new('RGB', (100, 100))
        with io.BytesIO() as output:
            image.save(output, format='PNG')
            image_bytes = output.getvalue()

        # Act
        GetTextRead(io.BytesIO(image_bytes))

        # Assert
        mock_print.assert_called_once_with('\n')

if __name__ == '__main__':
    unittest.main()