var G = go.GraphObject.make;
var myDiagram;

// 生成GUID
var guid = function() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
};

// 关闭当前对话框
var closeCurrDlg = function() {
    $(this).dialog('closeCurrent');
};

var jsonNewStep = { key: guid(), text: "新步骤", activityType: 2 };

// 显示编辑“流程”的对话框
var showEditFlowDlg = function(title) {
    $(this).dialog({
        width: 700,
        height: 400,
        url: gUrlEditFlow,
        title: title,
        onClose:function() {
            window.isCreateFlow = false;
        }
    });
};

// 显示编辑“步骤”或“连线”的对话框
var showEditNode = function (node) {
    window.gNodeData = node.data;

    var url = "";
    var title = "";
    if (node instanceof go.Link) {
        if (!window.gNodeData.key) {
            window.gNodeData.key = guid();
        }
        url = gUrlEditLink;
        title = '编辑条件';
    } else if ((node instanceof go.Node) && node.data.figure !== 'Circle') {
        url = gUrlEditStep;
        title = '编辑步骤';
    } else{
        $(this).alertmsg('warn', "开始和结束步骤不可编辑~");
    }

    $(this).dialog({
        width: 700,
        height: 400,
        url: url,
        title: title,
        onClose: function () {
            // 更改属性
            updateNodeData(node.data, window.gNodeData);
        }
    });
};

// 更新节点信息
var updateNodeData = function (oldData, newData) {
    myDiagram.startTransaction("vacate");
    for (var item in newData) {
        if (!newData.hasOwnProperty(item)) continue;
        if (item === "text") {
            myDiagram.model.setDataProperty(oldData, item, newData.text + " "); // bug:一定要加非空字符串才能更改text显示
        } else {
            myDiagram.model.setDataProperty(oldData, item, newData[item]);
        }
    }
    myDiagram.commitTransaction("vacate");
};

// 创建新步骤
var doCreateStep = function () {
    jsonNewStep.loc = "270 140";// “新步骤”显示的位置
    myDiagram.model.addNodeData(jsonNewStep);
};

// 检验流程图是否规范
var checkFlowDiagram = function(jsonDiagram) {
    var errMsg = "";

    //var flowData = window.gFlowData;
    if (getFlowName() === "") {;
        return '请设置流程名称！';
    }

    if (getTableSource() === "") {
        return '请设置流程数据源！';
    }

    if (!jsonDiagram) return "流程图不能为空~";

    // 检查
    if (jsonDiagram.nodeDataArray) {
        $.each(jsonDiagram.nodeDataArray, function (i, item) {

            // 处理类型为“角色”的步骤，必须包含角色
            if (item.dealType == 2 && (!item.hasOwnProperty("roleIds") || !item.roleIds)) {
                errMsg = "请为步骤【" + item.text + "】设置角色~";
                return false;
            }

            // 退回类型为“退回某一步”的步骤，必须包含步骤key
            if (item.backType == 3 && (!item.hasOwnProperty("backStep") || !item.backStep)) {
                errMsg = "请为步骤【" + item.text + "】指定退回步骤~";
                return false;
            }
        });
    }

    return errMsg;
};

// 打开流程图
var openFlow = function(flowId) {
    if (flowId <= 0) return;

    $.get(gUrlGetFlowData, { id: flowId }, function (flow) {

        if (!flow) {
            $(this).alertmsg('warn', "获取流程图数据出错~");
            return;
        }

        try {
            closeCurrDlg();
        }catch(ex) {}

        setFlowId(flow.Id);
        setFlowName(flow.Name);
        setTableSource(flow.TableSource);
        setProcessContent(flow.ProcessContent);
        setRemark(flow.Remark);

        myDiagram.model = go.Model.fromJson(flow.ProcessContent);
        loadDiagramProperties();
    });
};

var initFlowDiagram = function() {
    
    myDiagram =
        G(go.Diagram, "flowdiv", // must name or refer to the DIV HTML element
        {
            grid: G(go.Panel, "Grid",
                G(go.Shape, "LineH", { stroke: "lightgray", strokeWidth: 0.5 }),
                G(go.Shape, "LineH", { stroke: "gray", strokeWidth: 0.5, interval: 10 }),
                G(go.Shape, "LineV", { stroke: "lightgray", strokeWidth: 0.5 }),
                G(go.Shape, "LineV", { stroke: "gray", strokeWidth: 0.5, interval: 10 })
            ),
            allowDrop: true, // must be true to accept drops from the Palette
            allowTextEdit: false,
            allowHorizontalScroll: false,
            allowVerticalScroll: false,
            "clickCreatingTool.archetypeNodeData": jsonNewStep, // 双击创建新步骤
            "draggingTool.dragsLink": true,
            "draggingTool.isGridSnapEnabled": true,
            "linkingTool.isUnconnectedLinkValid": true,
            "linkingTool.portGravity": 20,
            "relinkingTool.isUnconnectedLinkValid": true,
            "relinkingTool.portGravity": 20,
            "relinkingTool.fromHandleArchetype":
                G(go.Shape, "Diamond", { segmentIndex: 0, cursor: "pointer", desiredSize: new go.Size(8, 8), fill: "tomato", stroke: "darkred" }),
            "relinkingTool.toHandleArchetype":
                G(go.Shape, "Diamond", { segmentIndex: -1, cursor: "pointer", desiredSize: new go.Size(8, 8), fill: "darkred", stroke: "tomato" }),
            "linkReshapingTool.handleArchetype":
                G(go.Shape, "Diamond", { desiredSize: new go.Size(7, 7), fill: "lightblue", stroke: "deepskyblue" }),
            "undoManager.isEnabled": true
        });

    // 流程图如果有变动，则提示用户保存
    myDiagram.addDiagramListener("Modified", function(e) {
        var button = document.getElementById("btnSaveFlow");
        if (button) button.disabled = !myDiagram.isModified;
        var idx = document.title.indexOf("*");
        if (myDiagram.isModified) {
            if (idx < 0) document.title += "*";
        } else {
            if (idx >= 0) document.title = document.title.substr(0, idx);
        }
    });

    // 双击事件
    myDiagram.addDiagramListener("ObjectDoubleClicked", function(ev) {
        var part = ev.subject.part;
        showEditNode(part);
    });

    // 创建连接点
    var makeNodePort = function(name, spot, output, input) {
        // the port is basically just a small transparent square
        return G(go.Shape, "Circle",
        {
            fill: null, // not seen, by default; set to a translucent gray by showSmallPorts, defined below
            stroke: null,
            desiredSize: new go.Size(7, 7),
            alignment: spot, // align the port on the main Shape
            alignmentFocus: spot, // just inside the Shape
            portId: name, // declare this object to be a "port"
            fromSpot: spot,
            toSpot: spot, // declare where links may connect at this port
            fromLinkable: output,
            toLinkable: input, // declare whether the user may draw links to/from here
            cursor: "pointer" // show a different cursor to indicate potential link point
        });
    };

    // 选中节点的样式
    var nodeSelectionAdornmentTemplate =
        G(go.Adornment, "Auto",
            G(go.Shape, { fill: null, stroke: "deepskyblue", strokeWidth: 1.5, strokeDashArray: [4, 2] }),
            G(go.Placeholder)
        );

    // 生成右键菜单项
    var makeMenuItem = function(text, action, visiblePredicate) {
        return G("ContextMenuButton",
            G(go.TextBlock, text, {
                margin: 5,
                textAlign: "left",
                stroke: "#555555"
            }),
            { click: action },
            // don't bother with binding GraphObject.visible if there's no predicate
            visiblePredicate ? new go.Binding("visible", "", visiblePredicate).ofObject() : {});
    };

    // 右键菜单
    var partContextMenu =
        G(go.Adornment, "Vertical",
            makeMenuItem("编辑",
                function(e, obj) { // OBJ is this Button
                    var contextmenu = obj.part; // the Button is in the context menu Adornment
                    var part = contextmenu.adornedPart; // the adornedPart is the Part that the context menu adorns
                    // now can do something with PART, or with its data, or with the Adornment (the context menu)
                    showEditNode(part);
                }),
            makeMenuItem("剪切",
                function(e, obj) { e.diagram.commandHandler.cutSelection(); },
                function(o) { return o.diagram.commandHandler.canCutSelection(); }),
            makeMenuItem("复制",
                function(e, obj) { e.diagram.commandHandler.copySelection(); },
                function(o) { return o.diagram.commandHandler.canCopySelection(); }),
            makeMenuItem("删除",
                function(e, obj) { e.diagram.commandHandler.deleteSelection(); },
                function(o) { return o.diagram.commandHandler.canDeleteSelection(); })
        );

    // tooltip上显示的信息
    var nodeInfo = function(d) {
        return '双击或单击右键可编辑';
    };

    // 是否显示步骤的连接点
    var showNodePort = function (node, show) {
        node.ports.each(function (port) {
            if (port.portId !== "") { // don't change the default port, which is the big shape
                port.fill = show ? "rgba(0,0,0,.3)" : null;
            }
        });
    };

    // 步骤图的样式模板
    myDiagram.nodeTemplate =
        G(go.Node, "Spot",
            { locationSpot: go.Spot.Center },
            new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
            { selectable: true, selectionAdornmentTemplate: nodeSelectionAdornmentTemplate },
            new go.Binding("angle").makeTwoWay(),
            // the main object is a Panel that surrounds a TextBlock with a Shape
            G(go.Panel, "Auto",
                { name: "PANEL" },
                new go.Binding("desiredSize", "size", go.Size.parse).makeTwoWay(go.Size.stringify),
                G(go.Shape, "RoundedRectangle", // 默认形状
                    {
                        portId: "", // the default port: if no spot on link data, use closest side
                        fromLinkable: true,
                        toLinkable: true,
                        cursor: "pointer",
                        fill: "#7e7e7f", // 默认颜色
                        strokeWidth: 1,
                        stroke: "#DDDDDD"
                    },
                    new go.Binding("figure"),
                    new go.Binding("fill")),
                G(go.TextBlock,
                    {
                        font: "bold 11pt Helvetica, Arial, sans-serif",
                        margin: 8,
                        maxSize: new go.Size(160, NaN),
                        wrap: go.TextBlock.WrapFit,
                        editable: true,
                        stroke: "white"
                    },
                    new go.Binding("text").makeTwoWay()), // the label shows the node data's text
                {
                    toolTip:// this tooltip Adornment is shared by all nodes
                        G(go.Adornment, "Auto",
                            G(go.Shape, { fill: "#FFFFCC" }),
                            G(go.TextBlock, { margin: 4 }, // the tooltip shows the result of calling nodeInfo(data)
                                new go.Binding("text", "", nodeInfo))
                        ),
                    // 绑定上下文菜单
                    contextMenu: partContextMenu
                }
            ),
            // 4个连接点
            makeNodePort("T", go.Spot.Top, false, true),
            makeNodePort("L", go.Spot.Left, true, true),
            makeNodePort("R", go.Spot.Right, true, true),
            makeNodePort("B", go.Spot.Bottom, true, false),
            {
                mouseEnter: function (e, node) { showNodePort(node, true); },
                mouseLeave: function (e, node) { showNodePort(node, false); }
            }
        );

    var linkSelectionAdornmentTemplate =
        G(go.Adornment, "Link",
            G(go.Shape,
                // isPanelMain declares that this Shape shares the Link.geometry
                { isPanelMain: true, fill: null, stroke: "deepskyblue", strokeWidth: 0 }) // use selection object's strokeWidth
        );

    // 定义连接线的样式模板
    myDiagram.linkTemplate =
        G(go.Link, // the whole link panel
            { selectable: true, selectionAdornmentTemplate: linkSelectionAdornmentTemplate },
            { relinkableFrom: true, relinkableTo: true, reshapable: true },
            {
                routing: go.Link.AvoidsNodes,
                curve: go.Link.JumpOver,
                corner: 5,
                toShortLength: 4
            },
            G(go.Shape, // 线条
            { stroke: "black" }),
            G(go.Shape, // 箭头
            { toArrow: "standard", stroke: null }),
            G(go.Panel, "Auto",
                G(go.Shape, // 标签背景色
                {
                    fill: null,
                    stroke: null
                }),
                G(go.TextBlock, // 标签文本
                    {
                        textAlign: "center",
                        font: "10pt helvetica, arial, sans-serif",
                        stroke: "#555555",
                        margin: 4
                    },
                    new go.Binding("text", "text")), // the label shows the node data's text
                {
                    toolTip:// this tooltip Adornment is shared by all nodes
                        G(go.Adornment, "Auto",
                            G(go.Shape, { fill: "#FFFFCC" }),
                            G(go.TextBlock, { margin: 4 }, // the tooltip shows the result of calling nodeInfo(data)
                                new go.Binding("text", "", nodeInfo))
                        ),
                    // this context menu Adornment is shared by all nodes
                    contextMenu: partContextMenu
                }
            )
        );

    // 初始化图例面板
    var myPalette =
        G(go.Palette, "myPaletteDiv", // must name or refer to the DIV HTML element
        {
            maxSelectionCount: 1,
            nodeTemplateMap: myDiagram.nodeTemplateMap, // share the templates used by myDiagram
            linkTemplate: // simplify the link template, just in this Palette
                G(go.Link,
                    {
                        locationSpot: go.Spot.Center,
                        selectionAdornmentTemplate:
                            G(go.Adornment, "Link",
                                { locationSpot: go.Spot.Center },
                                G(go.Shape,
                                { isPanelMain: true, fill: null, stroke: "deepskyblue", strokeWidth: 0 }),
                                G(go.Shape, // the arrowhead
                                { toArrow: "Standard", stroke: null })
                            )
                    },
                    {
                        routing: go.Link.AvoidsNodes,
                        curve: go.Link.JumpOver,
                        corner: 5,
                        toShortLength: 4
                    },
                    new go.Binding("points"),
                    G(go.Shape, // the link path shape
                    { isPanelMain: true, strokeWidth: 2 }),
                    G(go.Shape, // the arrowhead
                    { toArrow: "Standard", stroke: null })
                ),
            model: new go.GraphLinksModel([
                { key: guid(), text: "开始", figure: "Circle", fill: "#4fba4f", activityType: 1 },
                jsonNewStep,
                { key: guid(), text: "结束", figure: "Circle", fill: "#CE0620", activityType: 4 }
            ])
        });
};

// 保存之前，先设置流程图的其它属性
var saveDiagramProperties = function() {
    myDiagram.model.modelData.position = go.Point.stringify(myDiagram.position);
};

// 加载之前，先设置位置
var loadDiagramProperties = function(e) {
    var pos = myDiagram.model.modelData.position;
    if (pos) myDiagram.initialPosition = go.Point.parse(pos);
};

// 新建流程
var onCreateFlow = function () {

    var flowName = getFlowName();
    if (flowName && myDiagram.isModified) {
        $(this).alertmsg('confirm', '当前流程图未保存，确认要新建吗？', {
            okCall: function () {
                myDiagram.model = go.Model.fromJson({});// 清空流程图
                doCreateFlow();
            }
        });
    } else {
        // 如果有打开的流程，则先清空流程图
        //if (window.gFlowData && window.gFlowData.id) {
        //    myDiagram.model = go.Model.fromJson({});
        //}
        doCreateFlow();
    }
};

var doCreateFlow = function () {
    window.isCreateFlow = true;
    showEditFlowDlg("新建流程");
};

// 设置流程属性
var onSetFlow = function () {

    if (!getFlowName()) {
        onCreateFlow();
        return;
    }

    showEditFlowDlg("流程属性");
};

var getFlowId = function() {
   return $("#formSaveFlow #id").val();
};

var getFlowName = function () {
    return $("#formSaveFlow #name").val();
};

var getTableSource = function () {
    return $("#formSaveFlow #tableSource").val();
};

var getProcessContent = function() {
    return $("#formSaveFlow #processContent").val();
};

var getRemark = function() {
    return $("#formSaveFlow #remark").val();
};

var setFlowId = function (id) {
    $("#formSaveFlow #id").val(id);
};

var setFlowName = function (name) {
    $("#formSaveFlow #name").val(name);
};

var setTableSource = function (source) {
    $("#formSaveFlow #tableSource").val(source);
};

var setProcessContent = function (content) {
    $("#formSaveFlow #processContent").val(content);
};

var setRemark = function(remark) {
    $("#formSaveFlow #remark").val(remark);
};

