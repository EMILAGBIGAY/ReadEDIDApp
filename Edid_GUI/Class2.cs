class process
{
    static void HeaderInfo(string edidData, string file)
    {
        string header = edidData.Substring(0, 16);
        string manufacturerID = edidData.Substring(16, 4);
        ushort ManValue = Convert.ToUInt16(manufacturerID, 16);

        int ManLetter1 = (ManValue & 0x7C00) >> 10;
        char AscMan1 = Convert.ToChar(ManLetter1 + 64);

        int ManLetter2 = (ManValue & 0x03E0) >> 5;
        char AscMan2 = Convert.ToChar(ManLetter2 + 64);

        int ManLetter3 = ManValue & 0x001F;
        char AscMan3 = Convert.ToChar(ManLetter3 + 64);

        string ManId = String.Concat(AscMan1, AscMan2, AscMan3);

        //16-18 manufacturer name
        string productIDCodeLSB = edidData.Substring(20, 2);
        string productIDCodeMSB = edidData.Substring(22, 2);
        //20-22 LSB
        //22-24 MSB
        string serialNumber02 = edidData.Substring(24, 2);
        string serialNumber24 = edidData.Substring(26, 2);
        string serialNumber46 = edidData.Substring(28, 2);
        string serialNumber68 = edidData.Substring(30, 2);

        string manufactureDateWeek = edidData.Substring(32, 2);
        string manufactureDateYear = edidData.Substring(34, 2);
        //32-34 week
        //34-36 year
        string edidVersion = edidData.Substring(36, 2);
        string edidRevision = edidData.Substring(38, 2);

        using (StreamWriter writer = new StreamWriter(file, true))
        {
            writer.WriteLine("Header: " + header);
            writer.WriteLine("Manufacturer ID: " + ManId);
            writer.WriteLine("Product ID Code: " + productIDCodeMSB + productIDCodeLSB + "h");
            writer.WriteLine("Serial Number: " + serialNumber68 + serialNumber46 + serialNumber24 + serialNumber02 + "h");

            int ManufactureDateWeekDecimal = int.Parse(manufactureDateWeek, System.Globalization.NumberStyles.HexNumber);
            writer.WriteLine("Manufacture Week: " + ManufactureDateWeekDecimal);

            int ManufactureDateYearDecimal = int.Parse(manufactureDateYear, System.Globalization.NumberStyles.HexNumber);
            writer.WriteLine("Manufacture Year: " + (ManufactureDateYearDecimal + 1990));
            writer.WriteLine("EDID Version #: " + edidVersion);
            writer.WriteLine("EDID Revision #: " + edidRevision);
        }

    }
    static void BasicDisplayParameters(string edidData, string file)
    {
        string videoInputType = edidData.Substring(0, 1);
        //40 1 = digital, 0 = analog
        //41 digital:interface 
        string horizontalSize = edidData.Substring(2, 2);
        string verticalSize = edidData.Substring(4, 2);
        string displayGamma = edidData.Substring(6, 2);

        using (StreamWriter writer = new StreamWriter(file, true))
        {
            int videoInputTypeDecimal = int.Parse(videoInputType, System.Globalization.NumberStyles.HexNumber);
            int videoInputInterface = int.Parse(edidData.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
            if (videoInputTypeDecimal >= 8)
            {
                writer.WriteLine("Video Input Type: Digital");

                if (videoInputTypeDecimal == 9)
                {
                    writer.WriteLine("Color Bit Depth: 6 Bits per Primary Color");
                }
                else if (videoInputTypeDecimal == 10)
                {
                    writer.WriteLine("Color Bit Depth: 8 Bits per Primary Color");
                }
                else if (videoInputTypeDecimal == 11)
                {
                    writer.WriteLine("Color Bit Depth: 10 Bits per Primary Color");
                }
                else if (videoInputTypeDecimal == 12)
                {
                    writer.WriteLine("Color Bit Depth: 12 Bits per Primary Color");
                }
                else if (videoInputTypeDecimal == 13)
                {
                    writer.WriteLine("Color Bit Depth: 14 Bits per Primary Color");
                }
                else if (videoInputTypeDecimal == 14)
                {
                    writer.WriteLine("Color Bit Depth: 16 Bits per Primary Color");
                }
                else if (videoInputTypeDecimal == 15)
                {
                    writer.WriteLine("Color Bit Depth: reserved");
                }

                if (videoInputInterface == 1)
                {
                    writer.WriteLine("DVI is supported");
                }
                else if (videoInputInterface == 2)
                {
                    writer.WriteLine("HDMIa is supported");
                }
                else if (videoInputInterface == 3)
                {
                    writer.WriteLine("HDMIb is supported");
                }
                else if (videoInputInterface == 4)
                {
                    writer.WriteLine("MDDI is supported");
                }
                else if (videoInputInterface == 5)
                {
                    writer.WriteLine("Display Port is supported");
                }
            }
            else
            {
                writer.WriteLine("Video Input Type: Analog");
            }

            int horizontalDecimal = int.Parse(horizontalSize, System.Globalization.NumberStyles.HexNumber);
            writer.WriteLine("Horizontal Size: " + horizontalDecimal + " cm");

            int verticalDecimal = int.Parse(verticalSize, System.Globalization.NumberStyles.HexNumber);
            writer.WriteLine("Vertical Size: " + verticalDecimal + " cm");

            double displayGammaDecimal = int.Parse(displayGamma, System.Globalization.NumberStyles.HexNumber);
            displayGammaDecimal = (displayGammaDecimal + 100) / 100;
            writer.WriteLine("Display Gamma: " + displayGammaDecimal);
        }

    }
    static void SupportedFeaturesBitmap(string edidData, string file)
    {
        string[] support = new string[8];
        string supportedFeatures = edidData.Substring(0, 2);
        //string supportedFeatures34 = edidData.Substring(49, 1); 

        //convert hex data to binary
        byte[] supportFeat = new byte[supportedFeatures.Length * 4]; // Binary array
        for (int i = 0; i < supportedFeatures.Length; i++)
        {

            byte value = (byte)(supportedFeatures[i] <= '9' ? supportedFeatures[i] - '0' : supportedFeatures[i] - 'A' + 10);
            int index = i * 4;

            supportFeat[index] = (byte)((value >> 3) & 1);
            supportFeat[index + 1] = (byte)((value >> 2) & 1);
            supportFeat[index + 2] = (byte)((value >> 1) & 1);
            supportFeat[index + 3] = (byte)(value & 1);
        }

        for (int i = 0; i < supportFeat.Length; i++)
        {
            if (supportFeat[i] == 0)
            {
                support[i] = "No";
            }
            else if (supportFeat[i] == 1)
            {
                support[i] = "Yes";
            }
        }

        string ColorValue = "";
        if (supportFeat[3] == 0 && supportFeat[4] == 0)
        {
            ColorValue = "RGB:4:4:4";
        }
        else if (supportFeat[3] == 0 && supportFeat[4] == 1)
        {
            ColorValue = "RGB:4:4:4 + YCrCb:4:4:4";
        }
        else if (supportFeat[3] == 1 && supportFeat[4] == 0)
        {
            ColorValue = "RGB:4:4:4 + YCrCb:4:2:2";
        }
        else if (supportFeat[3] == 1 && supportFeat[4] == 1)
        {
            ColorValue = "RGB:4:4:4 + YCrCb:4:4:4 + YCrCb:4:2:2";
        }

        using (StreamWriter writer = new StreamWriter(file, true))
        {
            writer.WriteLine("Display Frequency is continuous frequency: " + support[7]);
            writer.WriteLine("Preferred Timing Mode includes the native pixel format and preferred refresh rate of display device: " + support[6]);
            writer.WriteLine("sRGB Standard is the default color space: " + support[5]);
            writer.WriteLine("Supported Color Encoding Formats: " + ColorValue);
            writer.WriteLine("Active Off: " + support[2]);
            writer.WriteLine("Suspend Mode: " + support[1]);
            writer.WriteLine("Standby Mode: " + support[0]);
        }

    }
    static void ChromaticCoordinates(string edidData, string file)
    {
        string Rsig = edidData.Substring(0, 1);//25     EE 1110 1110      Red X 1010 0011 11
        string Gsig = edidData.Substring(1, 1);
        string Bsig = edidData.Substring(2, 1);//26     95 1001 0101      Red Y 0101 0100 10 
        string Wsig = edidData.Substring(3, 1);
        string Redx = edidData.Substring(4, 2);//27     A3 1010 0011    Green X 0100 1100 11
        string Redy = edidData.Substring(6, 2);//28     54 0101 0100    Green Y 1001 1011 10
        string GreenX = edidData.Substring(8, 2);//g x  4C 0100 1100     Blue X 0010 0110 10
        string GreenY = edidData.Substring(10, 2);//     99 1001 1011     Blue Y 0000 1111 01
        string BlueX = edidData.Substring(12, 2);//      26 0010 0110    White X 0101 0000 01
        string BlueY = edidData.Substring(14, 2);//      0F 0000 1111    White Y 0101 0100 01
        string WhiteX = edidData.Substring(16, 2);//     50 0101 0000
        string WhiteY = edidData.Substring(18, 2);//     54 0101 0100

        double[] ChromeValues = new double[8];

        //Red X
        int RBinary = Convert.ToInt32(Rsig, 16);
        string Rbits = Convert.ToString(RBinary, 2).PadLeft(4, '0');
        string Redxbits = Rbits.Substring(0, 2);
        string RedxBinary = Convert.ToString(Convert.ToInt32(Redx, 16), 2);
        RedxBinary += Redxbits;
        long RedxDecimal = Convert.ToInt64(RedxBinary, 2);
        double RedxDouble = (double)RedxDecimal / 1024;

        ChromeValues[0] = Math.Round(RedxDouble, 5);

        //Red Y
        string RedYbits = Rbits.Substring(2, 2);
        string RedYBinary = Convert.ToString(Convert.ToInt32(Redy, 16), 2);
        RedYBinary += RedYbits;
        long RedYDecimal = Convert.ToInt64(RedYBinary, 2);
        double RedYDouble = (double)RedYDecimal / 1024;

        ChromeValues[1] = Math.Round(RedYDouble, 5);

        //Green X
        int GBinary = Convert.ToInt32(Gsig, 16);
        string Gbits = Convert.ToString(GBinary, 2).PadLeft(4, '0');
        string Greenxbits = Gbits.Substring(0, 2);
        string GreenxBinary = Convert.ToString(Convert.ToInt32(GreenX, 16), 2);
        GreenxBinary += Greenxbits;
        long GreenxDecimal = Convert.ToInt64(GreenxBinary, 2);     //red x deliverable
        double GreenxDouble = (double)GreenxDecimal / 1024;

        ChromeValues[2] = Math.Round(GreenxDouble, 5);

        //Green Y
        string GreenYbits = Gbits.Substring(2, 2);
        string GreenYBinary = Convert.ToString(Convert.ToInt32(GreenY, 16), 2);
        GreenYBinary += GreenYbits;
        long GreenYDecimal = Convert.ToInt64(GreenYBinary, 2);     //red x deliverable
        double GreenYDouble = (double)GreenYDecimal / 1024;

        ChromeValues[3] = Math.Round(GreenYDouble, 5);

        //Blue X
        int BBinary = Convert.ToInt32(Bsig, 16);
        string Bbits = Convert.ToString(BBinary, 2).PadLeft(4, '0');
        string Bluexbits = Bbits.Substring(0, 2);
        string BluexBinary = Convert.ToString(Convert.ToInt32(BlueX, 16), 2);
        BluexBinary += Bluexbits;
        long BluexDecimal = Convert.ToInt64(BluexBinary, 2);     //red x deliverable
        double BluexDouble = (double)BluexDecimal / 1024;

        ChromeValues[4] = Math.Round(BluexDouble, 5);

        //Blue Y
        string BlueYbits = Bbits.Substring(2, 2);
        string BlueYBinary = Convert.ToString(Convert.ToInt32(BlueY, 16), 2);
        BlueYBinary += BlueYbits;
        long BlueYDecimal = Convert.ToInt64(BlueYBinary, 2);     //red x deliverable
        double BlueYDouble = (double)BlueYDecimal / 1024;

        ChromeValues[5] = Math.Round(BlueYDouble, 5);

        //White X
        int WBinary = Convert.ToInt32(Wsig, 16);
        string Wbits = Convert.ToString(WBinary, 2).PadLeft(4, '0');
        string Whitexbits = Wbits.Substring(0, 2);
        string WhitexBinary = Convert.ToString(Convert.ToInt32(WhiteX, 16), 2);
        WhitexBinary += Whitexbits;
        long WhitexDecimal = Convert.ToInt64(WhitexBinary, 2);
        double WhitexDouble = (double)WhitexDecimal / 1024;

        ChromeValues[6] = Math.Round(WhitexDouble, 5);

        //White Y
        string WhiteYbits = Wbits.Substring(2, 2);
        string WhiteYBinary = Convert.ToString(Convert.ToInt32(WhiteY, 16), 2);
        WhiteYBinary += WhiteYbits;
        long WhiteYDecimal = Convert.ToInt64(WhiteYBinary, 2);
        double WhiteYDouble = (double)WhiteYDecimal / 1024;

        ChromeValues[7] = Math.Round(WhiteYDouble, 5);

        using (StreamWriter writer = new StreamWriter(file, true))
        {
            writer.WriteLine("Red X Value: " + ChromeValues[0]);
            writer.WriteLine("Red Y Value: " + ChromeValues[1]);
            writer.WriteLine("Green X Value: " + ChromeValues[2]);
            writer.WriteLine("Green Y Value: " + ChromeValues[3]);
            writer.WriteLine("Blue X Value: " + ChromeValues[4]);
            writer.WriteLine("Blue Y Value: " + ChromeValues[5]);
            writer.WriteLine("White X Value: " + ChromeValues[6]);
            writer.WriteLine("White Y Value: " + ChromeValues[7]);
        }

    }
    static void TimingsInfo(string edidData, string refresh, string file)
    {
        string establishedSupportTimings = edidData.Substring(0, 4);
        string manufacturerReservedTiming = edidData.Substring(4, 2);
        string edidStandardTimingsSupported = edidData.Substring(6, 32);
        string StdTime = edidData.Substring(8, 2);

        string TimeBits = Convert.ToString(Convert.ToInt32(StdTime, 16), 2).PadLeft(8, '0');

        //string AspRatio = TimeBits.Substring(0, 2);

        // string AspOut = "";
        // if(AspRatio == "00"){
        //     AspOut = "16:10";
        // }else if(AspRatio == "01"){
        //     AspOut = "4:3";
        // }else if(AspOut == "10"){
        //     AspOut = "5:4";
        // }else if(AspOut == "11"){
        //     AspOut = "16:9";
        // }

        using (StreamWriter writer = new StreamWriter(file, true))
        {
            writer.WriteLine("Established Support Timings: " + establishedSupportTimings);
            writer.WriteLine("Manufacturer's Reserved Timing: " + manufacturerReservedTiming);
            writer.WriteLine("EDID Standard Timings Supported: " + edidStandardTimingsSupported);
            writer.WriteLine("Refresh Rate: " + (Convert.ToInt32(refresh, 16) + 60) + "(Hz)");
        }

    }
    static void ProcessTimingBlock(string TimingBlock, string file)
    {
        string pixelClockLsb = TimingBlock.Substring(0, 2);
        string pixelClockMsb = TimingBlock.Substring(2, 2);
        pixelClockMsb += pixelClockLsb;
        double pixelDouble = (double)Convert.ToInt32(pixelClockMsb, 16);
        pixelDouble /= 100;

        //2-4
        string HorizontalActiveWidth = TimingBlock.Substring(8, 1) + TimingBlock.Substring(4, 2);
        string HorizontalBlankingWidth = TimingBlock.Substring(9, 1) + TimingBlock.Substring(6, 2);

        //5-7
        string VerticalActiveWidth = TimingBlock.Substring(14, 1) + TimingBlock.Substring(10, 2);
        string VerticalBlankingWidth = TimingBlock.Substring(15, 1) + TimingBlock.Substring(12, 2);

        //8-11
        string eleventhBit = Convert.ToString(Convert.ToInt32(TimingBlock.Substring(22, 2), 16), 2).PadLeft(8, '0');

        string HorizontalFrontPorch = eleventhBit.Substring(0, 2) + string.Join(string.Empty, TimingBlock.Substring(16, 2).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        string HorizontalSyncPulse = eleventhBit.Substring(2, 2) + string.Join(string.Empty, TimingBlock.Substring(18, 2).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

        string VerticalFrontPorch = eleventhBit.Substring(4, 2) + string.Join(string.Empty, TimingBlock.Substring(20, 1).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        string VerticalSyncPulse = eleventhBit.Substring(6, 2) + string.Join(string.Empty, TimingBlock.Substring(21, 1).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

        //12-14
        string HorizontalImageSize = TimingBlock.Substring(28, 1) + TimingBlock.Substring(24, 2);
        string VerticalImageSize = TimingBlock.Substring(29, 1) + TimingBlock.Substring(26, 2);

        //15-16
        string HorizontalBorder = TimingBlock.Substring(30, 2);
        string VerticalBorder = TimingBlock.Substring(32, 2);

        //17
        string FeaturesBitmap = Convert.ToString(Convert.ToInt32(TimingBlock.Substring(34, 2), 16), 2).PadLeft(8, '0');
        string[] SyncInfo = new string[5];

        //7
        if (FeaturesBitmap[0] == '1')
        {
            SyncInfo[0] = "Interlaced: Yes";
        }
        else if (FeaturesBitmap[0] == '0')
        {
            SyncInfo[0] = "Interlaced: no";
        }

        //6-5 & 0
        if (FeaturesBitmap[7] == '0')
        {
            if (FeaturesBitmap[1] == '0')
            {
                if (FeaturesBitmap[2] == '0')
                    SyncInfo[1] = "Stereo Viewing Support: none";
                else if (FeaturesBitmap[2] == '1')
                    SyncInfo[1] = "Stereo Viewing Support: field sequential, right during stereo sync";
            }
            else if (FeaturesBitmap[1] == '1')
            {
                if (FeaturesBitmap[2] == '0')
                    SyncInfo[1] = "Stereo Viewing Support: field sequential, left during stereo sync";
                if (FeaturesBitmap[2] == '1')
                    SyncInfo[1] = "Stereo Viewing Support: 4-way interleaved";
            }
        }
        else if (FeaturesBitmap[7] == '1')
        {
            if (FeaturesBitmap[1] == '0')
            {
                SyncInfo[1] = "Stereo Viewing Support: 2-way interleaved, right image on even lines";
            }
            else if (FeaturesBitmap[1] == '1')
            {
                if (FeaturesBitmap[2] == '0')
                    SyncInfo[1] = "Stereo Viewing Support: 2-way interleaved, left image on even lines";
                if (FeaturesBitmap[2] == '1')
                    SyncInfo[1] = "Stereo Viewing Support: side-by-side interleaved";
            }
        }

        //4-3
        if (FeaturesBitmap[3] == '0')
        {
            if (FeaturesBitmap[4] == '0')
            {
                SyncInfo[2] = "Sync Type: Analog Composite";
            }
            else if (FeaturesBitmap[4] == '1')
            {
                SyncInfo[2] = "Sync Type: Bipolar Analog Composite";
            }

            if (FeaturesBitmap[5] == '0')
            {
                SyncInfo[3] = "Serration: Without";
            }
            else if (FeaturesBitmap[5] == '1')
            {
                SyncInfo[3] = "Serration: with serrations (H-sync during V-sync)";
            }

            if (FeaturesBitmap[6] == '0')
            {
                SyncInfo[4] = "Sync on red and blue lines additionally to green: sync on green signal only";
            }
            else if (FeaturesBitmap[6] == '1')
            {
                SyncInfo[4] = "Sync on red and blue lines additionally to green: sync on all three (RGB) video signals";
            }
        }
        else if (FeaturesBitmap[3] == '1' && FeaturesBitmap[4] == '0')
        {
            SyncInfo[2] = "Sync Type: Digital Composite";

            if (FeaturesBitmap[5] == '0')
            {
                SyncInfo[3] = "Serration: Without";
            }
            else if (FeaturesBitmap[5] == '1')
            {
                SyncInfo[3] = "Serrations: with serrations (H-sync during V-sync)";
            }

            if (FeaturesBitmap[6] == '0')
            {
                SyncInfo[4] = "Horizontal Sync: Negative";
            }
            else if (FeaturesBitmap[6] == '1')
            {
                SyncInfo[4] = "Horizontal Sync: Positive";
            }
        }
        else if (FeaturesBitmap[3] == '1' && FeaturesBitmap[4] == '1')
        {
            SyncInfo[2] = "Sync Type: Digital Separate";

            if (FeaturesBitmap[5] == '0')
            {
                SyncInfo[3] = "Vertical Sync: Negative";
            }
            else if (FeaturesBitmap[5] == '1')
            {
                SyncInfo[3] = "Vertical Sync: Positive";
            }

            if (FeaturesBitmap[6] == '0')
            {
                SyncInfo[4] = "Horizontal Sync: Negative";
            }
            else if (FeaturesBitmap[6] == '1')
            {
                SyncInfo[4] = "Horizontal Sync: Positive";
            }
        }

        using (StreamWriter writer = new StreamWriter(file, true))
        {
            writer.WriteLine("Detailed Timing: ");
            writer.WriteLine("Pixel Clock: " + "{0:0.00}", pixelDouble + " MHz");
            writer.WriteLine("Horizontal Active Width: " + Convert.ToInt32(HorizontalActiveWidth, 16) + " pixels");
            writer.WriteLine("Horizontal Blanking Width: " + Convert.ToInt32(HorizontalBlankingWidth, 16) + " pixels");
            writer.WriteLine("Horizontal Addressable Width: " + Convert.ToInt32(HorizontalImageSize, 16) + " mm");
            writer.WriteLine("Horizontal Front Porch: " + Convert.ToInt32(HorizontalFrontPorch, 2) + " pixels");
            writer.WriteLine("Horizontal Sync Pulse: " + Convert.ToInt32(HorizontalSyncPulse, 2) + " pixels");
            writer.WriteLine("Horizontal Border: " + Convert.ToInt32(HorizontalBorder, 16) + " pixels");

            writer.WriteLine("Vertical Active Width: " + Convert.ToInt32(VerticalActiveWidth, 16) + " lines");
            writer.WriteLine("Vertical Blanking Width: " + Convert.ToInt32(VerticalBlankingWidth, 16) + " lines");
            writer.WriteLine("Vertical Addressable Width: " + Convert.ToInt32(VerticalImageSize, 16) + " mm");
            writer.WriteLine("Vertical Front Porch: " + Convert.ToInt32(VerticalFrontPorch, 2) + " lines");
            writer.WriteLine("Vertical Sync Pulse: " + Convert.ToInt32(VerticalSyncPulse, 2) + " lines");
            writer.WriteLine("Vertical Border: " + Convert.ToInt32(VerticalBorder, 16) + " lines");
            writer.WriteLine("Sync Signal Information:");
            for (int i = 0; i < SyncInfo.Length; i++)
            {
                writer.WriteLine(SyncInfo[i]);
            }
        }


    }
    static void AplhaData(string edidData, string file)
    {
        char[] alph = new char[edidData.Length / 2];
        int count = 4;
        int index = 0;
        if (edidData.Substring(0, 2) == "FE")
        {
            while (edidData.Substring(count, 2) != "0A" && edidData.Substring(count, 2) != "00" && count <= edidData.Length - 1)
            {
                alph[index] = Convert.ToChar(Convert.ToByte(edidData.Substring(count, 2), 16));
                index++;
                count += 2;
            }
            int nullCount = Array.IndexOf(alph, '\0');
            if (nullCount >= 0)
            {
                Array.Resize(ref alph, nullCount);
            }
            using (StreamWriter writer = new StreamWriter(file, true))
            {
                writer.Write("Alpha numeric String: ");
                writer.WriteLine(alph);
            }
        }
    }
    static void ManuDisplayDescriptor(string edidData, string file)
    {
        if (edidData.Substring(6, 2) == "00")
        {
            using (StreamWriter writer = new StreamWriter(file, true))
            {
                writer.WriteLine("Manufacturer (00h) Display Descriptor: " + edidData);
            }
        }

    }
    static void ExtraFlags(string edidData, string file)
    {
        string extensionFlag = edidData.Substring(0, 2);
        string checksum = edidData.Substring(2, 2);

        using (StreamWriter writer = new StreamWriter(file, true))
        {
            writer.WriteLine("Extension Flag: " + extensionFlag + "h");
            writer.WriteLine("Checksum: " + checksum + "h");
        }

    }
    static void CalculateChecksum(string hexData, string file)
    {
        if (hexData == null)
            throw new ArgumentNullException(nameof(hexData));

        if (hexData.Length != 256)
            throw new ArgumentException("Hex data length must be 256 characters.", nameof(hexData));

        byte[] data = new byte[128];
        for (int i = 0; i < 128; i++)
        {
            data[i] = Convert.ToByte(hexData.Substring(i * 2, 2), 16);
        }

        byte checksum = 0;
        string pF = "";
        for (int i = 0; i < data.Length; i++)
        {
            checksum += data[i];
        }

        if(checksum == 0)
        {
            pF = "(Pass)";
        }
        else
        {
            pF = "(Fail)";
        }

        using (StreamWriter writer = new StreamWriter(file, true))
        {
            writer.WriteLine("Manual Checksum: " + checksum + " "+pF);
            writer.WriteLine();
        }

    }
    public void ParseEDID(string filePath, string EDIDInformation)
    {
        string edidData = File.ReadAllText(filePath);

        using (StreamWriter writer = new StreamWriter(EDIDInformation, true))
        {
            writer.WriteLine("EDID Information:");
            writer.Write('\n');
            int index = 0;
            int counter = 0;
            while (index < edidData.Length)
            {
                if (counter == 48)
                {
                    writer.WriteLine();
                    counter = 0;
                }

                writer.Write(edidData[index]);
                counter++;
                index++;
            }
        }

        edidData = edidData.Replace(" ", string.Empty);

        // Extract and format the different components of the EDID data
        
        string header = edidData.Substring(0, 40);
        string BasicDisplay = edidData.Substring(40, 8);
        string supportedFeatures = edidData.Substring(48, 2);
        string colorCharacteristics = edidData.Substring(50, 20);
        string StandardTimingInfo = edidData.Substring(70, 40);
        string MaxVertRef = edidData.Substring(0, 2);

        string detailedTimingDescriptorBlock1 = edidData.Substring(108, 36);
        string detailedTimingDescriptorBlock2 = edidData.Substring(144, 36);
        string detailedTimingDescriptorBlock3 = edidData.Substring(180, 36);
        string detailedTimingDescriptorBlock4 = edidData.Substring(216, 36);
        string NumericString = edidData.Substring(186, 34);
        string ManufactureDisplay = edidData.Substring(216, 36);
        string extensionFlag = edidData.Substring(252, 4);
        string manualChecksum = edidData.Substring(0, 256);


        //256


        //string secondCheckSum = edidData.Substring(510, 2);
        //string checksum = edidData.Substring(254, 2);
        AplhaData(NumericString, EDIDInformation);
        CalculateChecksum(manualChecksum, EDIDInformation);
        HeaderInfo(header, EDIDInformation);
      
        BasicDisplayParameters(BasicDisplay, EDIDInformation);
        SupportedFeaturesBitmap(supportedFeatures, EDIDInformation);
        ChromaticCoordinates(colorCharacteristics, EDIDInformation);
        TimingsInfo(StandardTimingInfo, MaxVertRef, EDIDInformation);

        //Console.WriteLine("Color Characteristics: " + colorCharacteristics);
        ProcessTimingBlock(detailedTimingDescriptorBlock1, EDIDInformation);
        ProcessTimingBlock(detailedTimingDescriptorBlock2, EDIDInformation);

        
        ManuDisplayDescriptor(ManufactureDisplay, EDIDInformation);
        // ProcessTimingBlock(detailedTimingDescriptorBlock3,EDIDInformation);
        // ProcessTimingBlock(detailedTimingDescriptorBlock4,EDIDInformation);

        ExtraFlags(extensionFlag, EDIDInformation);
        

        //Console.WriteLine("Detailed Timing Descriptor Block 1: " + detailedTimingDescriptorBlock1);
        //Console.WriteLine("Detailed Timing Descriptor Block 2: " + detailedTimingDescriptorBlock2);
        //Console.WriteLine("Detailed Timing Descriptor Block 3: " + detailedTimingDescriptorBlock3);
        //Console.WriteLine("Detailed Timing Descriptor Block 4: " + detailedTimingDescriptorBlock4);

        
    }

}
