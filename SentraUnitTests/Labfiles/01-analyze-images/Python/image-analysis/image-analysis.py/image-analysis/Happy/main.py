import unittest
from unittest.mock import patch, MagicMock
from io import BytesIO
from PIL import Image
import sys

class TestMain(unittest.TestCase):

    @patch('main.load_dotenv')
    def test_main_loads_environment_variables(self, mock_load_dotenv):
        # Arrange
        mock_load_dotenv.return_value = None

        # Act
        main()

        # Assert
        mock_load_dotenv.assert_called_once()

    @patch('main.Image.open')
    @patch('main.ImageDraw.Draw')
    @patch('main.sys.exit')
    def test_main_image_processing(self, mock_sys_exit, mock_draw, mock_open):
        # Arrange
        mock_image = MagicMock(spec=Image)
        mock_draw_instance = MagicMock(spec=ImageDraw)
        mock_open.return_value = mock_image
        mock_draw.return_value = mock_draw_instance

        # Act
        main()

        # Assert
        mock_open.assert_called_once()
        mock_draw.assert_called_once_with(mock_image)
        mock_sys_exit.assert_not_called()

    @patch('main.requests.get')
    @patch('main.pyplot.show')
    def test_main_data_retrieval_and_plotting(self, mock_show, mock_get):
        # Arrange
        mock_response = MagicMock()
        mock_response.json.return_value = {'data': [1, 2, 3]}
        mock_get.return_value = mock_response

        # Act
        main()

        # Assert
        mock_get.assert_called_once()
        mock_show.assert_called_once()

if __name__ == '__main__':
    unittest.main()