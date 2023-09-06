using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;

public class QrCodeRecenter : MonoBehaviour {

    [SerializeField]
    private ARSession _session;
    [SerializeField]
    private ARSessionOrigin _sessionOrigin;
    [SerializeField]
    private ARCameraManager _cameraManager;
    [SerializeField]
    private List<Target> _navigationTargetObjects = new List<Target>();



    private Texture2D cameraImageTexture;
    private IBarcodeReader reader = new BarcodeReader(); // create a barcode reader for instance

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            SetQrCodeRecenterTarget("Stairs");
        }
    }

    private void OnEnable()
    {
        _cameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisable()
    {
        _cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {


        if (!_cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            return;
        }

        var conversionParams = new XRCpuImage.ConversionParams
        {
            // Get the entire image.
            inputRect = new RectInt(0, 0, image.width, image.height),

            // Downsample by 2.
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

            // Choose RGBA format.
            outputFormat = TextureFormat.RGBA32,

            // Flip across the vertical axis (mirror image).
            transformation = XRCpuImage.Transformation.MirrorY
        };

        // See how many bytes you need to store the final image.
        int size = image.GetConvertedDataSize(conversionParams);

        // Allocate a buffer to store the image.
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        // Extract the image data
        image.Convert(conversionParams, buffer);

        // The image was converted to RGBA32 format and written into the provided buffer
        // so you can dispose of the XRCpuImage. You must do this or it will leak resources.
        image.Dispose();

        // At this point, you can process the image, pass it to a computer vision algorithm, etc.
        // In this example, you apply it to a texture to visualize it.

        // You've got the data; let's put it into a texture so you can visualize it.
        cameraImageTexture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false);

        cameraImageTexture.LoadRawTextureData(buffer);
        cameraImageTexture.Apply();

        // Done with your temporary data, so you can dispose it.
        buffer.Dispose();

        // Detect and decode the barcode inside the bitmap
        var result = reader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);

        // Do something with the result
        if (result != null)
        {
            SetQrCodeRecenterTarget(result.Text);
        }
    }

    private void SetQrCodeRecenterTarget(string targetText)
    {
        Target currentTarget = _navigationTargetObjects.Find(x => x.Name.ToLower().Equals(targetText.ToLower()));
        if (currentTarget != null) 
        {
            // Reset position and rotation of ARSession
            _session.Reset();

            // Add offset for recentering
            _sessionOrigin.transform.position = currentTarget.PositionObject.transform.position;
            _sessionOrigin.transform.rotation = currentTarget.PositionObject.transform.rotation;
            Debug.Log("QR Code Recenter");
        }
    }
    public void ChangeActiveFloor(string floorEntrance)
    {
        SetQrCodeRecenterTarget(floorEntrance);
        Debug.Log("Change Floor");
    }
}
